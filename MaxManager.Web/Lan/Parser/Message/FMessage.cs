namespace MaxManager.Web.Lan.Parser.Message
{
	public class FMessage : IMaxMessage
	{
		public string FirstNtpServerHost { get; set; }
		public string SecondNtpServerHost { get; set; }
	}
}