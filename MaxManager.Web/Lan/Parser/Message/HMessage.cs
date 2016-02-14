using System;

namespace MaxManager.Web.Lan.Parser.Message
{
	public class HMessage : IMaxMessage
	{
		public string SerialNumber { get; set; }
		public string RfAddress { get; set; }
		public string FirmwareVersion { get; set; }
		public DateTime CubeDateTime { get; set; }
		public string StateCubeTime { get; set; }
		public int NtpCounter { get; set; }
		public string Unknown { get; set; }
		public string FreeMemorySlots { get; set; }
		public string DutyCycle { get; set; }
		public string HttpConnectionId { get; set; }
	}
}