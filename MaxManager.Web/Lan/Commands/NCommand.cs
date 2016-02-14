using System;

namespace MaxManager.Web.Lan.Commands
{
	class NCommand : IMaxCommand
	{
		public NCommand()
		{
			Body = "n:";
		}

		public NCommand(TimeSpan timeout)
		{
			Body = "n:" + timeout.TotalSeconds.ToString("{0:x}");
		}

		public string Body { get; }
	}
}