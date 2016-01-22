namespace MaxManager.Web.State
{
	public class MaxDevice
	{
		public MaxDeviceType Type { get; set; }

		public string RadioAddress { get; set; }

		public string SerialNumber { get; set; }

		public string Name { get; set; }

		public string RadioState { get; set; }

		public DeviceState State { get; set; }

		public string StateInfo { get; set; }

		public MaxRoom Room { get; set; }

		public bool IsRadioOk => IsRadioStateOk && !State.TransmitError;

		public bool IsRadioStateOk => RadioState =="Ok";
	}
}
