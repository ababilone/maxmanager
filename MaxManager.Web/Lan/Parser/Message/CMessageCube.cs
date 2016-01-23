namespace MaxManager.Web.Lan.Parser.Message
{
	public class CMessageCube : CMessage
	{
		public bool IsPortalEnabled { get; set; }
		public string PortalUrl { get; set; }
		public byte[] TimeZoneWinter { get; set; }
		public byte[] TimeZoneDaylightSavings { get; set; }
	}
}