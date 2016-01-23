using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Merger
{
	class HMessageMerger : MessageMerger<HMessage>
	{
		protected override void Merge(MaxCube maxCube, HMessage message)
		{
			maxCube.CubeDateTime = message.CubeDateTime;
			maxCube.FirmwareVersion = message.FirmwareVersion;
			maxCube.RfAddress= message.RfAddress;
			maxCube.SerialNumber = message.SerialNumber;
			maxCube.NtpCounter = message.NtpCounter;
		}
	}
}