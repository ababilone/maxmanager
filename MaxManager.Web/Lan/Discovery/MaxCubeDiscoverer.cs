using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace MaxManager.Web.Lan.Discovery
{
	public class MaxCubeDiscoverer : IMaxCubeDiscoverer
	{
		private readonly DatagramSocket _datagramSocket;
		private readonly int _discoveryPort = 23272;
		private readonly HostName _discoveryHostName = new HostName("224.0.0.1");
		//private readonly HostName _discoveryHostName = new HostName("ff02::fb");
		
		public MaxCubeDiscoverer()
		{
			_datagramSocket = new DatagramSocket();
			_datagramSocket.MessageReceived += datagramSocket_MessageReceived;
		}

		public event CubeDiscoveredEventHandler CubeDiscovered;

		public async Task DiscoverCubes()
		{
			await BindAndJoin();
			await SendDiscoveryMessage();
		}

		private async Task SendDiscoveryMessage()
		{
			try
			{
				using (var outputStream = await _datagramSocket.GetOutputStreamAsync(_discoveryHostName, _discoveryPort.ToString()))
				{
					using (var dataWriter = new DataWriter(outputStream))
					{
						dataWriter.WriteString("eQ3Max*.**********I");
						await dataWriter.StoreAsync();
					}
				}
			}
			catch (Exception e)
			{
			}
		}

		private async Task BindAndJoin()
		{
			await _datagramSocket.BindServiceNameAsync(_discoveryPort.ToString());
			_datagramSocket.JoinMulticastGroup(_discoveryHostName);
		}

		private async void datagramSocket_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
		{
			if (args.LocalAddress == args.RemoteAddress)
				return;

			byte[] payload;
			using (var dataReader = new DataReader(args.GetDataStream()))
			{
				var count = await dataReader.LoadAsync(26);
				if (count != 26)
					return;

				payload = new byte[dataReader.UnconsumedBufferLength];
				dataReader.ReadBytes(payload);
			}

			var cubeDiscoveredEventArgs = new CubeDiscoveredEventArgs
			{
				RemoteAddress = args.RemoteAddress,
				CubeInfo = ParsePayload(payload)
			};

			CubeDiscovered?.Invoke(this, cubeDiscoveredEventArgs);
		}

		private CubeInfo ParsePayload(byte[] payload)
		{
			var name = Encoding.UTF8.GetString(payload.Take(8).ToArray());
			var serialNumber = Encoding.UTF8.GetString(payload.Skip(8).Take(10).ToArray());
			var requestId = Encoding.UTF8.GetString(payload.Skip(18).Take(1).ToArray()); ;
			var requestType = Encoding.UTF8.GetString(payload.Skip(19).Take(1).ToArray());

			return new CubeInfo
			{
				SerialNumber = serialNumber,
				Name = name,
				RequestId = requestId,
				RequestType = requestType,
			};
		}
	}
}