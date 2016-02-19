using MaxManager.Web.Lan.Events;

namespace MaxManager.Web.Lan
{
	public interface IMaxStateAnalyzer
	{
		event StateUpdatedEventHandler StateUpdated;
	}
}