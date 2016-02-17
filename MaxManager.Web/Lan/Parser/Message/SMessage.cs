namespace MaxManager.Web.Lan.Parser.Message
{
	public class SMessage : IMaxMessage
	{
		public int DutyCycle { get; set; }
		public bool CommandResult { get; set; }
		public int FreeMemorySlot { get; set; }

		public override string ToString()
		{
			return CommandResult ? "Command Succeed" : "Command Discarded";
		}
	}
}