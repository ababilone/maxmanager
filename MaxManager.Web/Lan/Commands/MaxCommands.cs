namespace MaxManager.Web.Lan.Commands
{
	public static class MaxCommands
	{
		public static readonly IMaxCommand L = new LMaxCommand();
		public static readonly IMaxCommand Q = new QMaxCommand();
	}
}