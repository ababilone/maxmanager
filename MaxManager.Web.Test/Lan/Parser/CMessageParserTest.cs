using MaxManager.Web.Lan.Parser;
using MaxManager.Web.Lan.Parser.Message;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace MaxManager.Web.Test.Lan.Parser
{
	[TestClass]
	public class CMessageParserTest
	{
		[TestMethod]
		public void TestParseCubeMessage()
		{
			var cMessageParser = new CMessageParser();
			var o = cMessageParser.Parse("C:0e8ecf,7Q6OzwATAf9LRVExMDY3NjU1AQsABEAAAAAAAAAAAP///////////////////////////wsABEAAAAAAAAAAQf///////////////////////////2h0dHA6Ly93d3cubWF4LXBvcnRhbC5lbHYuZGU6ODAvY3ViZQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAENFVAAACgADAAAOEENFU1QAAwACAAAcIA==");
			var cMessage = o as CMessageCube;

			Assert.IsInstanceOfType(o, typeof(CMessageCube));
		}

		[TestMethod]
		public void TestParceZMessage()
		{
			var cMessageParser = new CMessageParser();
			var o = cMessageParser.Parse("C:0ddca0,0g3coAEDEKBLSEEwMDExNjQ4KiE9CQcYAzAM/wA8SDxsPOQ9FT0gPSA9IEUgRSBFIEUgRSBFIEBLQG5AzkEgQSBBIEEgRSBFIEUgRSBFIEUgPEg8bDzkPRU9ID0gPSBFIEUgRSBFIEUgRSA8SDxsPOQ9FT0gPSA9IEUgRSBFIEUgRSBFIDxIPGw85D0VPSA9ID0gRSBFIEUgRSBFIEUgPEg8bDzkPRU9ID0gPSBFIEUgRSBFIEUgRSA8SDxsPOQ9FT0gPSA9IEUgRSBFIEUgRSBFIA==");
			var cMessage = o as CMessageHeatingThermostat;

			Assert.IsInstanceOfType(o, typeof(CMessageHeatingThermostat));
		}
	}
}