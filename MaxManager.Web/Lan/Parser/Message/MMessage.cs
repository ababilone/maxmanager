using System.Collections.Generic;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Parser.Message
{
	class MMessage : IMaxMessage
	{
		public List<MaxRoom> Rooms { get; set; }
		public List<MaxDevice> Devices { get; set; }

		public override string ToString()
		{
			return Rooms.Count + " rooms (" + Devices.Count + " devices)";
		}
	}
}