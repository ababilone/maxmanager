using MaxManager.Web.Lan.Events;
using MaxManager.Web.Lan.Merger;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan
{
	public class MaxStateAnalyzer : IMaxStateAnalyzer
	{
		public event StateUpdatedEventHandler StateUpdated;

		private readonly MaxMerger _maxMerger;
		private MaxCube _maxCube;

		public MaxStateAnalyzer(IMaxConnector maxConnector, MaxMerger maxMerger)
		{
			_maxMerger = maxMerger;

			maxConnector.Connected += (sender, args) => _maxCube = new MaxCube();
			maxConnector.MessageReceived += MaxConnector_MessageReceived;
		}

		private void MaxConnector_MessageReceived(object sender, MessageReceivedEventArgs messageReceivedEventArgs)
		{
			_maxMerger.Merge(_maxCube, messageReceivedEventArgs.MaxMessage);

			var stateUpdatedEventArgs = new StateUpdatedEventArgs { Rooms = _maxCube.Rooms };
			StateUpdated?.Invoke(this, stateUpdatedEventArgs);
		}
	}
}