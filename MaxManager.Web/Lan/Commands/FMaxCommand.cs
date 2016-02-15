namespace MaxManager.Web.Lan.Commands
{
	class FMaxCommand : IMaxCommand
	{
		public FMaxCommand()
		{
			Body = "f:\r\n";
		}

		public FMaxCommand(string firstNtpServerHost, string secondNtpServerHost)
		{
			Body = $"f:{firstNtpServerHost},{secondNtpServerHost}";
		}

		public string Body { get; }
	}
}
