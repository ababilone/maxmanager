using MaxManager.Web.Lan.Parser;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Merger
{
	abstract class MessageMerger<T> : IMessageMerger where T : class
	{
		public bool Accept(object message)
		{
			return message is T;
		}

		public void Merge(MaxCube maxCube, object message)
		{
			Merge(maxCube, message as T);	
		}

		protected abstract void Merge(MaxCube maxCube, T message);
	}
}