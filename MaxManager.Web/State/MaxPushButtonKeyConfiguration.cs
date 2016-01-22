namespace MaxManager.Web.State
{
    public class MaxPushButtonKeyConfiguration
    {
        public string Mode { get; set; }

        public float Temperature { get; set; }

        public override string ToString() {
            return Mode + " " + Temperature;
        }
    }
}
