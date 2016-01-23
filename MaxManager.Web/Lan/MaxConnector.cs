using System;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using MaxManager.Web.Lan.Merger;
using MaxManager.Web.Lan.Parser;
using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan
{
	public class MaxConnector : IDisposable
	{
		private readonly string _host;
		private readonly int _port = 62910;
		private readonly MaxParser _maxParser;
		private readonly MaxMerger _maxMerger;
		private MaxCube _maxCube;

		public MaxConnector(string host, MaxParser maxParser, MaxMerger maxMerger)
		{
			_host = host;
			_maxParser = maxParser;
			_maxMerger = maxMerger;
			_maxCube = new MaxCube();
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
						var count = await dataReader.LoadAsync(sizeof(char));
						if (count != sizeof(char))
							return;

						var readString = dataReader.ReadString(1);
						if (readString != "\n")
						{
							currentLine += readString;
						}
						else
						{
							var message = _maxParser.Parse(currentLine);
							_maxMerger.Merge(_maxCube, message);

							var stateUpdatedEventArgs = new StateUpdatedEventArgs
							{
								Rooms = _maxCube.Rooms,
							};
							StateUpdated?.Invoke(this, stateUpdatedEventArgs);

							currentLine = string.Empty;
							await Task.Delay(50);
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
