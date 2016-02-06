namespace MaxManager.Web.Lan.Serialization
{
	public class HeatingThermostatDeviceState
	{
		[MaxSerialization(BytePos = 1, BitPos = 7, BitSpan = 1, ReturnType = typeof(bool))]
		public bool IsBatteryLow { get; set; }

		[MaxSerialization(BytePos = 1, BitPos = 6, BitSpan = 1, ReturnType = typeof(bool))]
		public bool IsTransmitError { get; set; }

		[MaxSerialization(BytePos = 1, BitPos = 3, BitSpan = 1, ReturnType = typeof(bool))]
		public bool IsDaylightSaving { get; set; }

		[MaxSerialization(BytePos = 3, BitPos = 0, BitSpan = 6, ReturnType = typeof(int))]
		public int SetPointTemperature { get; set; }

		[MaxSerialization(BytePos = 1, BitPos = 0, BitSpan = 2, ReturnType = typeof(int))]
		public int RoomControlMode { get; set; }

		[MaxSerialization(BytePos = 4, BitPos = 0, BitSpan = 5, ReturnType = typeof(int))]
		public int VacationEndDay { get; set; }

		[MaxSerialization(BytePos = 4, BitPos = 5, BitSpan = 3, ReturnType = typeof(int))]
		public int VacationEndMonth { get; set; }

		[MaxSerialization(BytePos = 5, BitPos = 7, BitSpan = 1, ReturnType = typeof(int))]
		public int VacationEndMonthLsb { get; set; }

		[MaxSerialization(BytePos = 5, BitPos = 0, BitSpan = 6, ReturnType = typeof(int))]
		public int VacationEndYear { get; set; }

		[MaxSerialization(BytePos = 6, BitPos = 0, BitSpan = 6, ReturnType = typeof(int))]
		public int VacationEndTime { get; set; }
	}
}