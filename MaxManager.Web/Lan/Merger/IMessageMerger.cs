using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Merger
{
	interface IMessageMerger
	{
		bool Accept(object message);
		void Merge(MaxCube maxCube, object message);
	}
}