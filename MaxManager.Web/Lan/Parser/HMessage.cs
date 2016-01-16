namespace MaxManager.Web.Lan.Parser
{
	public class HMessage
	{
		public string SerialNumber { get; set; }
		public string RfAddress { get; set; }
		public string FirmwareVersion { get; set; }
		public string CubeDate { get; set; }
		public string StateCubeTime { get; set; }
		public string NtpCounter { get; set; }
		public string Unknown { get; set; }
		public string CubeTime { get; set; }
		public string FreeMemorySlots { get; set; }
		public string DutyCycle { get; set; }
		public string HttpConnectionId { get; set; }
	}
}