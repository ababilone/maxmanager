using System.Linq;
using MaxManager.Web.Lan.Parser.Message;
using MaxManager.Web.State;

namespace MaxManager.Web.Lan.Merger
{
	class MMessageMerger : MessageMerger<MMessage>
	{
		protected override void Merge(MaxCube maxCube, MMessage message)
		{
			foreach (var maxRoom in message.Rooms)
			{
				var existingRoom = maxCube.Rooms.SingleOrDefault(room => room.Id == maxRoom.Id);
				if (existingRoom != null)
					maxCube.Rooms.Remove(existingRoom);

				maxCube.Rooms.Add(maxRoom);
			}
		}
	}
}