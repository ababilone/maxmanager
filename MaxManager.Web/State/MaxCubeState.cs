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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxControl.State
{
    public class MaxCubeState : IMaxObject
    {

        public MaxCubeState()
        {
            Rooms = new List<MaxRoom>();
        }

        public String SerialNumber { get; set; }
        
        public int RfAddress { get; set; }
        
        public int FirmwareVersion { get; set; }
        
        public String CubeDate { get; set; }
        
        public long StateCubeTime { get; set; }
        
        public int NtpCounter { get; set; }
        
        public MaxCubeLastPing CubeLastPing { get; set; }
        
        public bool DaylightSaving { get; set; }
        
        public MaxHouse House { get; set; }
        
        public MaxPushButtonConfiguration PushButtonConfiguration { get; set; }

        public List<MaxRoom> Rooms { get; set; }

        public bool IsInSync() {
            return IsInSync(TimeSpan.FromSeconds(15));
        }

        public bool IsInSync(TimeSpan timeSpan) {
            var lastPingTime = CubeLastPing.Date;
            return DateTime.Now - lastPingTime < timeSpan;
        }

        public List<MaxDevice> GetAllDevicesWithTransmitError() {
            var ret = new List<MaxDevice>();
            ret.AddRange(House.GetDevicesWithError());
            foreach (var room in Rooms) {
                ret.AddRange(room.GetDevicesWithError());
            }
            return ret;
        }

        public List<MaxDevice> GetAllDevicesWithLowBattery() {
            var ret = new List<MaxDevice>();
            ret.AddRange(House.GetDevicesWithLowBattery());
            foreach (var room in Rooms) {
                ret.AddRange(room.GetDevicesWithLowBattery());
            }
            return ret;
        }

        public List<MaxDevice> GetAllDevices() {
            var ret = new List<MaxDevice>();
            ret.AddRange(House.Devices);
            foreach (var room in Rooms) {
                ret.AddRange(room.Devices);
            }
            return ret;
        }

        public List<MaxRoom> GetRoomsWithOpenWindow()
        {
            return Rooms.Where(room => room.IsWindowOpen()).ToList();
        }

        public List<String> GetRoomsWithOpenWindowAsRoomNames() {
            var rooms = GetRoomsWithOpenWindow();
            var ret = new List<String>(rooms.Count);
            ret.AddRange(rooms.Select(room => room.Name));
            return ret;
        }

        public override String ToString() {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FirmwareVersion);
            if (DaylightSaving) 
                stringBuilder.Append(" daylight-saving");
            return stringBuilder.ToString();
        }

        public void Wire() {
            foreach (var room in Rooms) {
                room.Wire(this);
            }
        }

        public float GetDefaultEcoTemperature() {
            var rooms = Rooms;
            return rooms.Count == 0 ? 17 : rooms.First().EcoTemperature;
        }

        public float GetDefaultComportTemperature() {
            var rooms = Rooms;
            return rooms.Count == 0 ? 22 : rooms.First().ComfortTemperature;
        }

        public List<float> GetSettableTemperatures() {
            var all= new List<float>();
            foreach (var room in Rooms) {
                all.AddRange(room.GetSettableTemperatures());
            }
            all.Sort();
            return all;
        }

    }
}
