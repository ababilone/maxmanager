/*
 * Copyright 2011 Witoslaw Koczewsi <wi@koczewski.de>
 * 
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU Affero
 * General Public License as published by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the
 * implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public
 * License for more details.
 * 
 * You should have received a copy of the GNU Affero General Public License along with this program. If not,
 * see <http://www.gnu.org/licenses/>.
 */

/**
 * http://www.fhemwiki.de/wiki/MAX
 *
 * http://www.schrankmonster.de/2012/08/17/reverse-engineering-the-elv-max-cube-protocol/
 *
 * http://blog.hekkers.net/2011/08/29/unravelling-the-elv-max-heating-control-system-protocol/
 *
 * https://github.com/Bouni/max-cube-protocol/blob/master/protocol.md
 */
// TODO broadcast to discover cubes

using System;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using MaxManager.Web.Lan.Parser;

namespace MaxManager.Web.Lan
{
	public class MaxConnector : IDisposable
	{
		private readonly string _host;
		private readonly MaxParser _maxParser;
		private readonly int _port = 62910;
		private readonly int _discoveryPort = 23272;
		private DatagramSocket _datagramSocket;

		public MaxConnector(string host, MaxParser maxParser)
		{
			_host = host;
			_maxParser = maxParser;
		}

		public async Task DiscoverCubes()
		{
			_datagramSocket = new DatagramSocket();
			_datagramSocket.MessageReceived += datagramSocket_MessageReceived;

			//await _datagramSocket.BindServiceNameAsync(_discoveryPort.ToString());

			using (var outputStream = await _datagramSocket.GetOutputStreamAsync(null, _discoveryPort.ToString()))
			{
				using (var dataWriter = new DataWriter(outputStream))
				{
					dataWriter.WriteString("eQ3Max*.**********I");
					await dataWriter.FlushAsync();
				}
			}
		}

		private async void datagramSocket_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
		{
			var remoteAddress = args.RemoteAddress;
			using (var dataReader = new DataReader(args.GetDataStream()))
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
						
						currentLine = string.Empty;
					}
				}

			}
		}

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
