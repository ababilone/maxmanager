namespace MaxManager.Web.State
{
    public class MaxPushButtonConfiguration
    {
        public MaxPushButtonKeyConfiguration UpperKey { get; set; }
        
        public MaxPushButtonKeyConfiguration LowerKey { get; set; }

        public override string ToString() {
            return GetType().Name;
        }
    }
}
