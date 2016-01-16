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
    public class MaxHeatingThermostatDeviceState : DeviceState 
    {    
        public float SetPointTemperature { get; set; }

        public float TemperatureOffset { get; set; }

        public static MaxHeatingThermostatDeviceState CreateDummy(int variant) {
            var dummy = new MaxHeatingThermostatDeviceState
            {
                SetPointTemperature = 42.23f,
                TemperatureOffset = variant == 0 ? 1 : 0,
                BatteryLow = false,
                TransmitError = false
            };
            return dummy;
        }

        public override String ToString() {
            return SetPointTemperature + " " + TemperatureOffset + " " + base.ToString();
        }
    }
}
