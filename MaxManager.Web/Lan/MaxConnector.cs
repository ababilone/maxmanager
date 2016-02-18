using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using MaxManager.Web.Components;
using MaxManager.Web.Lan.Commands;
using MaxManager.Web.Lan.Events;
using MaxManager.Web.Lan.Merger;
using MaxManager.Web.Lan.Parser;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan
{
	public class MaxConnector : IMaxConnector
	{
		private const string SocketId = "MaxSocket";

		private readonly int _port = 62910;
		private readonly MaxParser _maxParser;
		private readonly MaxMerger _maxMerger;
		private readonly MaxCube _maxCube;
		private StreamSocket _streamSocket;
		private IBackgroundTaskRegistration _backgroundTaskRegistration = null;

		public MaxConnector(MaxParser maxParser, MaxMerger maxMerger)
		{
			_maxParser = maxParser;
			_maxMerger = maxMerger;

			_maxCube = new MaxCube();

			ConnectionState = MaxConnectionState.Disconnected;
		}

		public event StateUpdatedEventHandler StateUpdated;
		public event MessageReceivedEventHandler MessageReceived;
		public event CommandSentEventHandler CommandSent;
		public event ConnectedEventHandler Connected;
		public event ExceptionThrowedEventHandler ExceptionThrowed;

		public MaxConnectionState ConnectionState { get; }

		private void CreateBackgroundTask()
		{
			if (_backgroundTaskRegistration != null)
				return;

			var socketActivityTrigger = new SocketActivityTrigger();
			var socketTaskBuilder = new BackgroundTaskBuilder
			{
				Name = "MaxManager.Web.Lan.SocketActivityTask",
				TaskEntryPoint = typeof(SocketActivityTask).FullName
			};
			socketTaskBuilder.SetTrigger(socketActivityTrigger);

			try
			{
				_backgroundTaskRegistration = socketTaskBuilder.Register();
			}
			catch (Exception e)
			{

			}
		}

		private void CreateStreamSocket()
		{
			if (_streamSocket != null)
				return;

			_streamSocket = new StreamSocket();
			//_streamSocket.EnableTransferOwnership(_backgroundTaskRegistration.TaskId, SocketActivityConnectedStandbyAction.Wake);
		}

		public async Task Connect(string host)
		{
			CreateBackgroundTask();

			CreateStreamSocket();

			await _streamSocket.ConnectAsync(new HostName(host), _port.ToString(), SocketProtectionLevel.PlainSocket);

			Connected?.Invoke(this, new ConnectedEventArgs { Host = host });

			await Process();
			//_streamSocket.TransferOwnership(SocketId);
		}

		private string _currentLine = string.Empty;
		private readonly List<string> _previousLines = new List<string>();

		public async Task Process()
		{
			if (_streamSocket == null)
				return;

			using (var dataReader = new DataReader(_streamSocket.InputStream))
			{
				while (true)
				{
					try
					{
						var count = await dataReader.LoadAsync(1);
						if (count != 1)
							return;

						var readString = dataReader.ReadString(1);
						if (readString != "\n")
						{
							_currentLine += readString;
						}
						else
						{
							var message = _maxParser.Parse(_currentLine);

							if (message != null)
								MessageReceived?.Invoke(this, new MessageReceivedEventArgs { When = DateTime.Now, MaxMessage = message });

							_maxMerger.Merge(_maxCube, message);

							var stateUpdatedEventArgs = new StateUpdatedEventArgs
							{
								Rooms = _maxCube.Rooms,
							};
							StateUpdated?.Invoke(this, stateUpdatedEventArgs);

							_previousLines.Add(_currentLine);
							_currentLine = string.Empty;
							await Task.Delay(50);
						}
					}
					catch (Exception e)
					{
						ExceptionThrowed?.Invoke(this, new ExceptionThrowedEventArgs { Exception = e });
						_currentLine = "";
					}
				}
			}
		}

		public async Task Send(IMaxCommand maxCommand)
		{
			if (_streamSocket == null)
				return;

			try
			{
				using (var dataWriter = new DataWriter(_streamSocket.OutputStream))
				{
					dataWriter.WriteString(maxCommand.Body);
					await dataWriter.StoreAsync();
					dataWriter.DetachStream();
				}
			}
			catch (Exception e)
			{
				ExceptionThrowed?.Invoke(this, new ExceptionThrowedEventArgs { Exception = e });
			}

			CommandSent?.Invoke(this, new CommandSentEventArgs { When = DateTime.Now, MaxCommand = maxCommand });

			await Process();
		}

		public void Dispose()
		{
			_streamSocket?.Dispose();
		}
	}
}
