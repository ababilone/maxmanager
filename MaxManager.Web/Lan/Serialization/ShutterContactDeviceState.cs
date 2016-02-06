namespace MaxManager.Web.Lan.Serialization
{
	public class ShutterContactDeviceState
	{
		[MaxSerialization(BytePos = 1, BitPos = 7, BitSpan = 1, ReturnType = typeof(bool))]
		public bool IsBatteryLow { get; set; }

		[MaxSerialization(BytePos = 1, BitPos = 6, BitSpan = 1, ReturnType = typeof(bool))]
		public bool IsTransmitError { get; set; }

		[MaxSerialization(1, 2, BytePos = 1, BitPos = 0, BitSpan = 2, ReturnType = typeof(bool))]
		public bool IsWindowOpen { get; set; }
	}
}