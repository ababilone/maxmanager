using System;
using System.Collections.Generic;

namespace MaxManager.Web.State
{
    public class MaxWeekTemperatureProfile
    {
	    public MaxWeekTemperatureProfile()
	    {
		    DayTemperatureProfiles = new List<MaxDayTemperatureProfile>();
	    }

        public List<MaxDayTemperatureProfile> DayTemperatureProfiles { get; set; }

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

        public override string ToString() {
            return DayTemperatureProfiles.Count.ToString();
        }
    }
}