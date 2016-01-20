/*
 * Copyright 2011 Witoslaw Koczewsi <wi@koczewski.de>
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the GNU Affero
 * General Public License as published by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the
 * implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public
 * License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License along with this program. If not,
 * see <http://www.gnu.org/licenses/>.
 */

namespace MaxManager.Web.State
{
	public class MaxDevice : IMaxObject
	{
		public MaxDeviceType Type { get; set; }

		public string RadioAddress { get; set; }

		public string SerialNumber { get; set; }

		public string Name { get; set; }

		public string RadioState { get; set; }

		public DeviceState State { get; set; }

		public string StateInfo { get; set; }

		public MaxRoom Room { get; set; }

		public bool IsRadioOk()
		{
			return IsRadioStateOk() && !State.TransmitError;
		}

		public bool IsRadioStateOk()
		{
			return "Ok".Equals(RadioState);
		}

		public string GetNameWithRoomName()
		{
			var r = Room;
			if (r == null)
				return Name;
			return r.Name + ", " + Name;
		}

		public bool IsDeviceTypeShutterContact()
		{
			return Type == MaxDeviceType.ShutterContact;
		}

		public bool IsDeviceTypeWallMountedThermostat()
		{
			return Type == MaxDeviceType.WallThermostat;
		}

		public override string ToString()
		{
			return Type + ": " + Name + " " + State;
		}

		public void Wire(MaxRoom room)
		{
			Room = room;
		}
	}
}
