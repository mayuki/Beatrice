namespace Beatrice.Request
{
    public class StatesPayload : IPayload
    {
        public Device[] Devices { get; set; }
        public class Device
        {
            public string Id { get; set; }
        }
    }
}
