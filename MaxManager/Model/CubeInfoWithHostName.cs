using Windows.Networking;
using MaxManager.Web.Lan.Discovery;

namespace MaxManager.Model
{
	public class CubeInfoWithHostName
	{
		public HostName HostName { get; set; }
		public CubeInfo CubeInfo { get; set; }
	}
}