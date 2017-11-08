namespace Beatrice.Request
{
    public partial class ExecutePayload : IPayload
    {
        public Command[] Commands { get; set; }

        public class Command
        {
            public Device[] Devices { get; set; }
            public ActionCommand[] Execution { get; set; }
        }

        public class Device
        {
            public string Id { get; set; }
        }
    }
}
