using System.Collections.Generic;
using MaxControl.State;

namespace MaxManager.Web.Lan
{
	public class StateUpdatedEventArgs
	{
		public List<MaxDevice> Devices { get; set; }
		public List<MaxRoom> Rooms { get; set; }
	}
}