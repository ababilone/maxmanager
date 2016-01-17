using Windows.Networking;

namespace MaxManager.Web.Lan.Discovery
{
	public class CubeDiscoveredEventArgs
	{
		public HostName RemoteAddress { get; set; }
		public CubeInfo CubeInfo { get; set; }
	}
}