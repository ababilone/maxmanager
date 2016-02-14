using System;
using MaxManager.Web.Lan.Parser.Message;

namespace MaxManager.Web.Lan.Events
{
	public class MessageReceivedEventArgs
	{
		public IMaxMessage MaxMessage { get; set; }
		public DateTime When { get; set; }
	}
}