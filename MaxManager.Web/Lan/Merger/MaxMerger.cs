using System.Collections.Generic;
using System.Linq;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Merger
{
	public class MaxMerger
	{
		private readonly List<IMessageMerger> _messageMergers;

		public MaxMerger()
		{
			_messageMergers = new List<IMessageMerger>
			{
				new HMessageMerger(),
				new MMessageMerger(),
				new CMessageMerger()
			};
		}

		public void Merge(MaxCube maxCube, object message)
		{
			var messageParser = _messageMergers.FirstOrDefault(merger => merger.Accept(message));
			messageParser?.Merge(maxCube, message);
		}
	}
}