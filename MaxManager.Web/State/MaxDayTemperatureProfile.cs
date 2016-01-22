using System;
using System.Collections.Generic;

namespace MaxManager.Web.State
{
    public class MaxDayTemperatureProfile
    {
        public MaxDayTemperatureProfile()
        {
            SwitchPoints = new List<MaxTemperatureProfilSwitchPoint>();
        }

        public DayOfWeek DayOfWeek { get; set; }
        
        public List<MaxTemperatureProfilSwitchPoint> SwitchPoints { get; set; }

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
    }
}
