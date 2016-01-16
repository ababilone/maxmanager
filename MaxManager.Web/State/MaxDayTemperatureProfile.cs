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

namespace MaxControl.State
{
    public class MaxDayTemperatureProfile : IMaxObject
    {
        public MaxDayTemperatureProfile()
        {
            SwitchPoints = new List<MaxTemperatureProfilSwitchPoint>();
        }

        public String DayOfWeek { get; set; }
        
        public List<MaxTemperatureProfilSwitchPoint> SwitchPoints { get; set; }

        public static MaxDayTemperatureProfile CreateDummy(int dayOfWeek) {
            var dummy = new MaxDayTemperatureProfile
            {
                DayOfWeek = dayOfWeek.ToString(),
                SwitchPoints =
                    new List<MaxTemperatureProfilSwitchPoint>
                    {
                        MaxTemperatureProfilSwitchPoint.CreateDummy(0),
                        MaxTemperatureProfilSwitchPoint.CreateDummy(1)
                    }
            };
            return dummy;
        }

        public MaxTemperatureProfilSwitchPoint GetSwitchPointForNow() {
            return GetSwitchPoint(DateTime.Now.TimeOfDay);
        }

        public MaxTemperatureProfilSwitchPoint GetSwitchPoint(TimeSpan time) {
            var points = new List<MaxTemperatureProfilSwitchPoint>(SwitchPoints);
            if (points.Count == 0)
                return null;

            points.Reverse();

            MaxTemperatureProfilSwitchPoint best = null;
            foreach (var sp in points) {
                if (best == null) {
                    best = sp;
                    continue;
                }
                var stop = sp.GetStopAsTime();
                if (stop < time)
                    return best;
                best = sp;
            }
            return best;
        }

        public override String ToString() {
            return DayOfWeek;
        }
    }
}
