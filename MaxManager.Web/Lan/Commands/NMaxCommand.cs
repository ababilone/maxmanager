using System;

namespace MaxManager.Web.Lan.Commands
{
	class NMaxCommand : IMaxCommand
	{
		public NMaxCommand()
		{
			Body = "n:\r\n";
		}

		public NMaxCommand(TimeSpan timeout)
		{
			Body = "n:" + timeout.TotalSeconds.ToString("{0:x}") + "\r\n";
		}

		public string Body { get; }
	}
}