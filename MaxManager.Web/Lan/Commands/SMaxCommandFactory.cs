namespace MaxManager.Web.Lan.Commands
{
	public class SMaxCommandFactory
	{
		public IMaxCommand CreateTemperatureAndModeCommand(string rfAddress, int roomId)
		{
			return new SMaxCommand("000440000000", rfAddress, roomId);
		}
	}
}