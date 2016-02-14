using System;
using System.Threading.Tasks;
using MaxManager.Web.Lan.Commands;

namespace MaxManager.Web.Lan
{
	public interface IMaxConnector : IDisposable
	{
		event StateUpdatedEventHandler StateUpdated;

		Task Connect(string host);

		Task Process();
		Task Send(IMaxCommand maxCommand);
	}
}