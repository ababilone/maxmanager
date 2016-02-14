using System;
using System.Threading.Tasks;
using MaxManager.Web.Lan.Commands;
using MaxManager.Web.Lan.Events;

namespace MaxManager.Web.Lan
{
	public interface IMaxConnector : IDisposable
	{
		event StateUpdatedEventHandler StateUpdated;

		Task Connect(string host);
		Task Process();
		Task Send(IMaxCommand maxCommand);

		event MessageReceivedEventHandler MessageReceived;
		event CommandSentEventHandler CommandSent;
		event ConnectedEventHandler Connected;
		event ExceptionThrowedEventHandler ExceptionThrowed;
	}
}