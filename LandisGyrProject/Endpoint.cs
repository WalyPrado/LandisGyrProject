namespace LandisGyrProject
{
    public class Endpoint
    {
        public string endpointSerialNumber { get; set; }
        public Enum.MeterModelIds meterModelId { get; set; }
        public int meterNumber { get; set; }
        public string meterFirmwareVersion { get; set; }
        public Enum.States switchState { get; set; }
    }
}
