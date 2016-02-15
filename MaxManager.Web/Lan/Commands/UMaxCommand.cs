namespace MaxManager.Web.Lan.Commands
{
	class UMaxCommand : IMaxCommand
	{
		public UMaxCommand(string portalUrl)
		{
			Body = $"u:{portalUrl}";
		}

		public string Body { get; }
	}
}