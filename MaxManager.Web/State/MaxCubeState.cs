using System.Collections.Generic;

namespace MaxManager.Web.State
{
    public class MaxCubeState
    {
        public MaxCubeState()
        {
            Rooms = new List<MaxRoom>();
        }

        public string SerialNumber { get; set; }
        
        public int RfAddress { get; set; }
        
        public int FirmwareVersion { get; set; }
        
        public string CubeDate { get; set; }
        
        public long StateCubeTime { get; set; }
        
        public int NtpCounter { get; set; }
        
        public bool DaylightSaving { get; set; }
        
        public MaxPushButtonConfiguration PushButtonConfiguration { get; set; }

        public List<MaxRoom> Rooms { get; set; }
    }
}