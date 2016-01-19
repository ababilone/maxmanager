namespace MaxManager.Web.Lan.Parser
{
	public class CMessage
	{
		public string RadioAddress { get; set; }
		public CMessageCube Cube { get; set; }
		public CMessageHeatingThermostat HeatingThermostat { get; set; }
		public CMessageWallThermostat WallThermostat { get; set; }
	}
}