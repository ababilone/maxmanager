using System;
using MaxManager.Web.Lan.Parser;
using MaxManager.Web.Lan.Serialization;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Commands
{
	public class SMaxCommand : IMaxCommand
	{
		public string Body { get; }

		[MaxSerialization(BitPos = 0, BytePos = 0, ByteSpan = 6, ReturnType = typeof(SCommand))]
		protected SCommand Command { get; set; }

		[MaxSerialization(BitPos = 0, BytePos = 6, ByteSpan = 3, ReturnType = typeof(MaxRfAddress))]
		public MaxRfAddress RfAddress { get; set; }

		[MaxSerialization(BitPos = 0, BytePos = 9, ByteSpan = 1, ReturnType = typeof(int))]
		public int RoomId { get; set; }

		protected enum SCommand : long
		{
			TemperatureAndMore = 0x000440000000
		}
	}

	public class STemperatureAndModeMaxCommand : SMaxCommand
	{
		public STemperatureAndModeMaxCommand()
		{
			Command = SCommand.TemperatureAndMore;
		}

		[MaxSerialization(BitPos = 6, BitSpan = 2, BytePos = 10, ReturnType = typeof(MaxRoomControlMode))]
		public MaxRoomControlMode Mode { get; set; }

		[MaxSerialization(BitPos = 0, BitSpan = 6, BytePos = 10, ReturnType = typeof(int))]
		public double Temperature { get; set; }

		[MaxSerialization(BitPos = 0, BytePos = 11, ByteSpan = 2, ReturnType = typeof(DateTime?))]
		public DateTime? DateUntil { get; set; }

		[MaxSerialization(BitPos = 0, BytePos = 13, ByteSpan = 1, ReturnType = typeof(TimeSpan?))]
		public TimeSpan? TimeUntil { get; set; }
	}
}