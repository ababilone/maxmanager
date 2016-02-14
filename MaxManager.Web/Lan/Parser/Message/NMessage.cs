using MaxManager.Web.Lan.Serialization;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Parser.Message
{
	public class NMessage : IMaxMessage
	{
		[MaxSerialization(BitPos = 0, BitSpan = 8, BytePos = 1, ReturnType = typeof(MaxDeviceType))]
		public MaxDeviceType DeviceType { get; set; }

		[MaxSerialization(BitPos = 0, BitSpan = 3 * 8, BytePos = 2, ReturnType = typeof(MaxRfAddress))]
		public MaxRfAddress RfAddress { get; set; }

		[MaxSerialization(BitPos = 0, BitSpan = 10 * 8, BytePos = 5, ReturnType = typeof(string))]
		public string SerialNumber { get; set; }
	}
}