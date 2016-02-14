using System;

namespace MaxManager.Model
{
	public class MaxEvent
	{
		public MaxEvent(string message)
		{
			When = DateTime.Now;
			Message = message;
		}

		public DateTime When { get; set; }
		public string Message { get; set; }
	}
}