namespace MaxManager.Web.State
{
    public class MaxHeatingThermostatDeviceState : DeviceState 
    {    
        public float SetPointTemperature { get; set; }

        public float TemperatureOffset { get; set; }
    }
}