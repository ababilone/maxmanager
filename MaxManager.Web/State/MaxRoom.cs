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

namespace MaxControl.State
{
    public class MaxRoom : IMaxObject
    {
        public MaxRoom()
        {
            Devices = new List<MaxDevice>();
        }

        public int Id { get; set; }
        
        public String Name { get; set; }
        
        public int Order { get; set; }
        
        public int BoostDuration { get; set; }
        
        public int BoostValveAngle { get; set; }
        
        public float ComfortTemperature { get; set; }
        
        public float EcoTemperature { get; set; }
        
        public float MaximumTemperature { get; set; }
        
        public float SetPointTemperature { get; set; }
        
        public bool SetPointTemperatureValid { get; set; }
        
        public String ControlMode { get; set; }
        
        public String DecalcificationDay { get; set; }
        
        public int DecalcificationHour { get; set; }
        
        public List<MaxDevice> Devices { get; set; }
        
        public bool StateChanged { get; set; }
        
        public bool StateDirty { get; set; }
        
        public bool TemperatureControllable { get; set; }
        
        public String TemperatureMode { get; set; }
        
        public DateTime TemporaryModeStopDate { get; set; }
        
        public MaxWeekTemperatureProfile WeekTemperatureProfile { get; set; }
        
        public int WindowOpenDuration { get; set; }
        
        public float WindowOpenTemperature { get; set; }
        
        public int MaximumNoOfHeatingThermostats { get; set; }
        
        public int MaximumNoOfShutterContacts { get; set; }
        
        public int MaximumNoOfWallMountedThermostats { get; set; }
        
        public float CurrentAutoTemperature { get; set; }
        
        public float ActualTemperature { get; set; }

        public MaxCubeState Cube { get; set; }

        public static MaxRoom CreateDummy(int variant) {
            var dummy = new MaxRoom
            {
                Id = variant,
                Name = "room " + variant,
                Order = variant,
                BoostDuration = 23,
                BoostValveAngle = 23,
                ComfortTemperature = 42.23f,
                EcoTemperature = 23.42f,
                MaximumTemperature = 99,
                SetPointTemperature = 33.33f,
                SetPointTemperatureValid = variant != 1,
                ControlMode = "controlMode",
                DecalcificationDay = "decalcificationDay",
                Devices = new List<MaxDevice> {MaxDevice.CreateDummyForRoom(0), MaxDevice.CreateDummyForRoom(1)},
                StateChanged = variant == 2,
                StateDirty = variant == 3,
                TemperatureControllable = variant == 4,
                TemperatureMode = "temperatureMode",
                TemporaryModeStopDate = new DateTime(DateTimeUtils.CurrentTimeMillis() + 12345678),
                WeekTemperatureProfile = MaxWeekTemperatureProfile.CreateDummy(variant),
                WindowOpenDuration = variant,
                WindowOpenTemperature = 11.11f,
                MaximumNoOfHeatingThermostats = 23,
                MaximumNoOfShutterContacts = 23,
                MaximumNoOfWallMountedThermostats = 23
            };
            return dummy;
        }

        public String GetUniqueId() {
            return Cube.SerialNumber + ":" + Id;
        }

        public List<MaxDevice> GetDevicesOfTypeWallMountedThermostat()
        {
            return Devices.Where(device => device.IsDeviceTypeWallMountedThermostat()).ToList();
        }

        public List<float> GetSettableTemperatures() {
            var ret = new List<float>();
            var max = MaximumTemperature;
            for (var i = 5; i <= max; i++) {
                ret.Add(Convert.ToSingle(i));
                var iPlus = i + 0.5f;
                if (iPlus <= max && iPlus > 17) {
                    ret.Add(Convert.ToSingle(iPlus));
                }
            }
            return ret;
        }

        public float? GetCurrentAutoTemperatureFromProfile() {
            var switchPoint = WeekTemperatureProfile.GetDayTemperatureProfileForToday().GetSwitchPointForNow();
            if (switchPoint == null) 
                return null;
            return switchPoint.Temperature;
        }

        public bool IsWindowOpen()
        {
            return Devices
                .Where(device => device.IsDeviceTypeShutterContact())
                .Select(device => (MaxShutterContactDeviceState) device.State)
                .Any(state => state.WindowOpen);
        }

        public bool IsAutoOrEco() {
            return IsControlModeAuto() || IsTemperatureModeEco();
        }

        public bool IsActualTemperatureAvailable() {
            return ActualTemperature > 0;
        }

        public bool IsTemperatureModeNormal() {
            return "Normal".Equals(TemperatureMode);
        }

        public bool IsTemperatureModeEco() {
            return "Eco".Equals(TemperatureMode);
        }

        public bool IsTemperatureModeComfort() {
            return "Comfort".Equals(TemperatureMode);
        }

        public List<MaxDevice> GetDevicesWithLowBattery()
        {
            return Devices.Where(device => device.State.BatteryLow).ToList();
        }

        public List<MaxDevice> GetDevicesWithError()
        {
            return Devices.Where(device => !device.IsRadioOk()).ToList();
        }

        public bool IsControlModeAuto() {
            return "Auto".Equals(ControlMode);
        }

        public bool IsControlModePermanently() {
            return "Permanently".Equals(ControlMode);
        }

        public bool IsControlModeTemporary() {
            return "Temporary".Equals(ControlMode);
        }

        public void Wire(MaxCubeState cube) {
            Cube = cube;
            foreach (var device in Devices) {
                device.Wire(this);
            }
        }
    }
}
