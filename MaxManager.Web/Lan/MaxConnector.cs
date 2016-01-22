using System;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using MaxManager.Web.Lan.Parser;
using MaxManager.Web.Lan.Parser.Message;

namespace MaxManager.Web.Lan
{
	public class MaxConnector : IDisposable
	{
		private readonly string _host;
		private readonly MaxParser _maxParser;
		private readonly int _port = 62910;

		public MaxConnector(string host, MaxParser maxParser)
		{
			_host = host;
			_maxParser = maxParser;
		}

		public event StateUpdatedEventHandler StateUpdated;

		public async Task LoadState()
		{
			using (var streamSocket = new StreamSocket())
			{
				await streamSocket.ConnectAsync(new HostName(_host), _port.ToString(), SocketProtectionLevel.PlainSocket);

				using (var dataReader = new DataReader(streamSocket.InputStream))
				{
					var currentLine = string.Empty;

					while (true)
					{
						var count = await dataReader.LoadAsync(sizeof (char));
						if (count != sizeof (char))
							return;

						var readString = dataReader.ReadString(1);
						if (readString != "\n")
						{
							currentLine += readString;
						}
						else
						{
							var message = _maxParser.Parse(currentLine);

							var mMessage = message as MMessage;
							if (mMessage != null)
							{
								
								var stateUpdatedEventArgs = new StateUpdatedEventArgs
								{
									Rooms = mMessage.Rooms,
									Devices = mMessage.Devices
								};
								StateUpdated?.Invoke(this, stateUpdatedEventArgs);
							}

							currentLine = string.Empty;
						}
					}
				}
			}
		}

		public void Dispose()
		{
			
		}
	}
}
