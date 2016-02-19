using System;
using System.Threading.Tasks;
using MaxManager.Web.Lan.Commands;
using MaxManager.Web.Lan.Events;

namespace MaxManager.Web.Lan
{
	public interface IMaxConnector : IDisposable
	{
		Task ConnectAsync(string host);
		Task SendAsync(IMaxCommand maxCommand);

		event MessageReceivedEventHandler MessageReceived;
		event CommandSentEventHandler CommandSent;
		event ConnectedEventHandler Connected;
		event ExceptionThrowedEventHandler ExceptionThrowed;
	}
}