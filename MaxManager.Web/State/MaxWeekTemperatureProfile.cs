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
    public class MaxWeekTemperatureProfile : IMaxObject
    {    
        public List<MaxDayTemperatureProfile> DayTemperatureProfiles { get; set; }

        public static MaxWeekTemperatureProfile CreateDummy(int variant) {
            var dummy = new MaxWeekTemperatureProfile
            {
                DayTemperatureProfiles = new List<MaxDayTemperatureProfile>()
            };
            for (var i = 0; i < 7; i++) {
                dummy.DayTemperatureProfiles.Add(MaxDayTemperatureProfile.CreateDummy(i));
            }
            return dummy;
        }

        public MaxDayTemperatureProfile GetDayTemperatureProfileForToday() {
            return GetDayTemperatureProfile(DateTime.Now.DayOfWeek);
        }

        public MaxDayTemperatureProfile GetDayTemperatureProfile(DayOfWeek weekday) {
            switch (weekday) {
                case DayOfWeek.Saturday:
                    return DayTemperatureProfiles[0];
                case DayOfWeek.Sunday:
                    return DayTemperatureProfiles[1];
                case DayOfWeek.Monday:
                    return DayTemperatureProfiles[2];
                case DayOfWeek.Tuesday:
                    return DayTemperatureProfiles[3];
                case DayOfWeek.Wednesday:
                    return DayTemperatureProfiles[4];
                case DayOfWeek.Thursday:
                    return DayTemperatureProfiles[5];
                case DayOfWeek.Friday:
                    return DayTemperatureProfiles[6];
            }
            throw new NotSupportedException("Unsupported weekday: " + weekday);
        }

        public List<MaxDayTemperatureProfile> GetDayTemperatureProfiles() {
            return DayTemperatureProfiles;
        }

        public override String ToString() {
            return GetDayTemperatureProfiles().Count.ToString();
        }
    }
}