using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Parser.Message
{
	public class LMessage
	{
		public MaxDeviceType DeviceType { get; set; }
		public string RfAddress { get; set; }
		public MaxRadioState RadioState { get; set; }
		public MaxStateInfo StateInfo { get; set; }
	}
}