using MaxManager.Web.Lan.Parser;
using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.State;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MaxManager.Web.Test.Lan.Parser
{
	[TestClass]
	public class NMessageParserTest
	{
		[TestMethod]
		public void TestParseNMessage()
		{
			var NMessagePayload = "N:Aw4VzExFUTAwMTUzNDD/";

			var nMessageParser = new NMessageParser();

			Assert.IsTrue(nMessageParser.Accept(NMessagePayload));

			var maxMessage = nMessageParser.Parse(NMessagePayload);

			Assert.IsInstanceOfType(maxMessage, typeof(NMessage));

			var nMessage = maxMessage as NMessage;

			Assert.AreEqual(MaxDeviceType.WallThermostat, nMessage.DeviceType);

			Assert.AreEqual("0E-15-CC", nMessage.RfAddress.HumanReadable);
			Assert.AreEqual(3, nMessage.RfAddress.Bytes.Length);
			Assert.AreEqual(14, nMessage.RfAddress.Bytes[0]);
			Assert.AreEqual(21, nMessage.RfAddress.Bytes[1]);
			Assert.AreEqual(204, nMessage.RfAddress.Bytes[2]);

			Assert.AreEqual("LEQ0015340", nMessage.SerialNumber);
		}
	}
}