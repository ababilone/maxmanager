using System;

namespace MaxManager.Web.State
{
    public class MaxTemperatureProfilSwitchPoint
    {    
        public DateTime Stop { get; set; }
        
        public double Temperature { get; set; }

        public TimeSpan GetStopAsTime() {
            return Stop.TimeOfDay;
        }

        public override string ToString() {
            return Stop.ToString("yyyy-MM-dd HH:mm:ss") + " " + Temperature;
        }
    }
}
