using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Parser.Message
{
	public class CMessage : IMaxMessage
	{
		public MaxDeviceType DeviceType { get; set; }
		public string RfAddress { get; set; }
	}
}