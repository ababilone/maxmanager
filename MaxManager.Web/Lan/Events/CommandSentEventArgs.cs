using System;
using MaxManager.Web.Lan.Commands;

namespace MaxManager.Web.Lan.Events
{
	public class CommandSentEventArgs
	{
		public DateTime When { get; set; }

		public IMaxCommand MaxCommand { get; set; }
	}
}