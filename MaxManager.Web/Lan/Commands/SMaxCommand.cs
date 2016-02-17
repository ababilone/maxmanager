using MaxManager.Web.Lan.Serialization;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Commands
{
	public class SMaxCommand : IMaxCommand
	{
		public string Body { get; }

		[MaxSerialization(BytePos = 0, ByteSpan = 6, ReturnType = typeof(SCommand))]
		public SCommand Command { get; set; }

		[MaxSerialization(BytePos = 6, ByteSpan = 3, ReturnType = typeof(MaxRfAddress))]
		public MaxRfAddress RfAddress { get; set; }

		[MaxSerialization(BytePos = 9, ByteSpan = 1, BitSpan = 8, ReturnType = typeof(int))]
		public int RoomId { get; set; }

		public enum SCommand : long
		{
			TemperatureAndMore = 0x000440000000
		}
	}
}