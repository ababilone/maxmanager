using System;
using System.Collections.Generic;

namespace MaxManager.Web.State
{
    public class MaxCube
    {
        public MaxCube()
        {
            Rooms = new List<MaxRoom>();
        }

        public string SerialNumber { get; set; }
        
        public string RfAddress { get; set; }
        
        public string FirmwareVersion { get; set; }
        
        public DateTime CubeDateTime { get; set; }
        
        public long StateCubeTime { get; set; }
        
        public int NtpCounter { get; set; }
        
        public bool DaylightSaving { get; set; }
        
        public MaxPushButtonConfiguration PushButtonConfiguration { get; set; }

        public List<MaxRoom> Rooms { get; set; }

	    public bool IsPortalEnabled { get; set; }

	    public string PortalUrl { get; set; }

	    public byte[] TimeZoneDaylightSavings { get; set; }

	    public byte[] TimeZoneWinter { get; set; }
    }
}