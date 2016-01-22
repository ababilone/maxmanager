namespace MaxManager.Web.State
{
    public class MaxShutterContactDeviceState : DeviceState 
    {
        public bool WindowOpen { get; set; }

        public override string ToString()
        {
	        return WindowOpen ? "windowOpen " + base.ToString() : base.ToString();
        }
    }
}
