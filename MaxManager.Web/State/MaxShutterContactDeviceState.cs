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
    public class MaxShutterContactDeviceState : DeviceState 
    {
        public bool WindowOpen { get; set; }

        public static MaxShutterContactDeviceState CreateDummy(int variant) {
            var dummy = new MaxShutterContactDeviceState
            {
                WindowOpen = variant == 1,
                BatteryLow = false,
                TransmitError = false
            };
            return dummy;
        }

        public override String ToString() {
            if (WindowOpen) 
                return "windowOpen " + base.ToString();
            return base.ToString();
        }
    }
}
