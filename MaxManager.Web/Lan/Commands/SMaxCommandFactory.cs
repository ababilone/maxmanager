using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Commands
{
	public class SMaxCommandFactory
	{
		public IMaxCommand CreateTemperatureAndModeCommand(MaxRfAddress rfAddress, int roomId)
		{
			return new STemperatureAndModeMaxCommand
			{
				RfAddress = rfAddress,
				RoomId = roomId
			};
		}
	}
}