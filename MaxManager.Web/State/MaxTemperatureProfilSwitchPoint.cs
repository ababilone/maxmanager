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
    public class MaxTemperatureProfilSwitchPoint : IMaxObject
    {    
        public DateTime Stop { get; set; }
        
        public float Temperature { get; set; }

        public static MaxTemperatureProfilSwitchPoint CreateDummy(int variant) {
            var dummy = new MaxTemperatureProfilSwitchPoint
            {
                Temperature = variant == 0 ? 23.42f : 42.23f,
                Stop =
                    new DateTime(variant == 0
                        ? DateTimeUtils.CurrentTimeMillis() - 10000
                        : DateTimeUtils.CurrentTimeMillis() + 10000)
            };
            return dummy;
        }

        public TimeSpan GetStopAsTime() {
            return Stop.TimeOfDay;
        }

        public override String ToString() {
            return DateUtils.FormatDateTime(Stop) + " " + Temperature;
        }
    }
}
