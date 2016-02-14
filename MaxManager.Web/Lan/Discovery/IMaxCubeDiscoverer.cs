using System.Threading.Tasks;

namespace MaxManager.Web.Lan.Discovery
{
	public interface IMaxCubeDiscoverer
	{
		event CubeDiscoveredEventHandler CubeDiscovered;
		Task DiscoverCubes();
	}
}