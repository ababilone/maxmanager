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

using System;

namespace MaxControl.State
{
    public class MaxDevice : IMaxObject
    {
        public String DeviceType { get; set; }
     
        public int RadioAddress { get; set; }
        
        public String SerialNumber { get; set; }
        
        public String Name { get; set; }
        
        public String RadioState { get; set; }
        
        public DeviceState State { get; set; }
        
        public String StateInfo { get; set; }
        
        public MaxRoom Room { get; set; }

        public static MaxDevice CreateDummyForRoom(int variant) {
            var dummy = new MaxDevice
            {
                DeviceType = "? roomDevice",
                Name = "dummy room device " + variant,
                RadioAddress = 0,
                RadioState = "radioState",
                SerialNumber = "serial123",
                State = variant == 0
                    ? (DeviceState) MaxShutterContactDeviceState.CreateDummy(variant)
                    : MaxHeatingThermostatDeviceState.CreateDummy(variant),
                StateInfo = "state info"
            };
            return dummy;
        }

        public static MaxDevice CreateDummyForHouse() {
            var dummy = new MaxDevice
            {
                DeviceType = "? houseDevice",
                Name = "dummy house device",
                RadioAddress = 0,
                RadioState = "radioState",
                SerialNumber = "serial123",
                State = MaxPushButtonDeviceState.CreateDummy(),
                StateInfo = "state info"
            };
            return dummy;
        }

        public bool IsRadioOk() {
            return IsRadioStateOk() && !State.TransmitError;
        }

        public bool IsRadioStateOk() {
            return "Ok".Equals(RadioState);
        }

        public String GetNameWithRoomName() {
            MaxRoom r = Room;
            if (r == null) 
                return Name;
            return r.Name + ", " + Name;
        }

        public bool IsDeviceTypeShutterContact() {
            return "ShutterContact".Equals(DeviceType);
        }

        public bool IsDeviceTypeWallMountedThermostat() {
            return "WallMountedThermostat".Equals(DeviceType);
        }

        public override String ToString() {
            return DeviceType + ": " + Name + " " + State;
        }

        public void Wire(MaxRoom room) {
            Room = room;
        }
    }
}
