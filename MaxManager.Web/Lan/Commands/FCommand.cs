namespace MaxManager.Web.Lan.Commands
{
	class FCommand : IMaxCommand
	{
		public FCommand()
		{
			Body = "f:\r\n";
		}

		public FCommand(string firstNtpServerHost, string secondNtpServerHost)
		{
			Body = $"f:{firstNtpServerHost},{secondNtpServerHost}";
		}

		public string Body { get; }
	}
}
