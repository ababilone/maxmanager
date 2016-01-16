using System.Collections.Generic;
using MaxControl.State;

namespace MaxManager.Web.Lan.Parser
{
	class MMessage
	{
		public List<MaxRoom> Rooms { get; set; }
		public List<MaxDevice> Devices { get; set; }
	}
}