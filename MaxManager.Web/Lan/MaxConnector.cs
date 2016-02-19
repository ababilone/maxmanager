using System;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using MaxManager.Web.Lan.Commands;
using MaxManager.Web.Lan.Events;
using MaxManager.Web.Lan.Merger;
using MaxManager.Web.Lan.Parser;
using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.State;
using MaxManager.Web.Utils;

namespace MaxManager.Web.Lan
{
	public class MaxConnector : IMaxConnector
	{
		private readonly int _port = 62910;
		private readonly MaxParser _maxParser;
		private readonly MaxMerger _maxMerger;
		private readonly MaxCube _maxCube;
		private StreamSocket _streamSocket;

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

		private void CreateStreamSocket()
		{
			if (_streamSocket != null)
				return;

			_streamSocket = new StreamSocket();
		}

		public async Task ConnectAsync(string host)
		{
			CreateStreamSocket();

			await _streamSocket.ConnectAsync(new HostName(host), _port.ToString(), SocketProtectionLevel.PlainSocket);

			Connected?.Invoke(this, new ConnectedEventArgs { Host = host });
			
			await InitialFetchesAsync();
		}

		public async Task SendAsync(IMaxCommand maxCommand)
		{
			if (_streamSocket == null)
				return;

			try
			{
				await _streamSocket.OutputStream.WriteLine(maxCommand.Body);
			}
			catch (Exception e)
			{
				ExceptionThrowed?.Invoke(this, new ExceptionThrowedEventArgs { Exception = e });
			}

			CommandSent?.Invoke(this, new CommandSentEventArgs { When = DateTime.Now, MaxCommand = maxCommand });

			await FetchMessageAsync();
		}

		private async Task InitialFetchesAsync()
		{
			while (true)
			{
				var message = await FetchMessageAsync();
				if (message is LMessages)
					break;
			}
		}

		private async Task<IMaxMessage> FetchMessageAsync()
		{
			if (_streamSocket == null)
				return null;

			try
			{
				var currentLine = await _streamSocket.InputStream.ReadLine();
				if (currentLine == null)
					return null;

				var message = _maxParser.Parse(currentLine);
				if (message != null)
					ProcessMessage(message);

				return message;
			}
			catch (Exception e)
			{
				ExceptionThrowed?.Invoke(this, new ExceptionThrowedEventArgs { Exception = e });
				return null;
			}
		}

		private void ProcessMessage(IMaxMessage message)
		{
			MessageReceived?.Invoke(this, new MessageReceivedEventArgs { When = DateTime.Now, MaxMessage = message });

			_maxMerger.Merge(_maxCube, message);

			var stateUpdatedEventArgs = new StateUpdatedEventArgs
			{
				Rooms = _maxCube.Rooms,
			};
			StateUpdated?.Invoke(this, stateUpdatedEventArgs);
		}

		public void Dispose()
		{
			_streamSocket?.Dispose();
		}
	}
}
