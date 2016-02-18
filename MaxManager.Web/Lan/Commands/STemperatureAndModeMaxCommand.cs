using System;
using MaxManager.Web.Lan.Parser;
using MaxManager.Web.Lan.Serialization;

namespace MaxManager.Web.Lan.Commands
{
	public class STemperatureAndModeMaxCommand : SMaxCommand
	{
		public STemperatureAndModeMaxCommand()
		{
			Command = SCommand.TemperatureAndMore;
		}

		[MaxSerialization(ByteSpan = 1, BitPos = 6, BitSpan = 2, BytePos = 10, ReturnType = typeof(MaxRoomControlMode))]
		public MaxRoomControlMode Mode { get; set; }

		[MaxSerialization(ByteSpan = 1, BitPos = 0, BitSpan = 6, BytePos = 10, ReturnType = typeof(int))]
		public int Temperature { get; set; }

		[MaxSerialization(BytePos = 11, ByteSpan = 2, ReturnType = typeof(DateTime?))]
		public DateTime? DateUntil { get; set; }

		[MaxSerialization(BytePos = 13, ByteSpan = 1, ReturnType = typeof(TimeSpan?))]
		public TimeSpan? TimeUntil { get; set; }

		public DateTime? Until => DateUntil?.Add(TimeUntil ?? TimeSpan.Zero);

		public override string ToString()
		{
			if (Mode == MaxRoomControlMode.Holiday)
			{
				return $"Setting room {RoomId} to t° {Temperature/2} (Mode {Mode} until {Until})";
            }
			return $"Setting room {RoomId} to t° {Temperature/2} (Mode {Mode})";
		}
	}
}