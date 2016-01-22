namespace MaxManager.Web.State
{
    public sealed class MaxIDeviceState : DeviceState 
    {
        public bool DisplayActualTemperature { get; set; }

        public float SetPointTemperature { get; set; }
        
        public float TemperatureOffset { get; set; }
        
        public float ActualTemperature { get; set; }
    }
}
