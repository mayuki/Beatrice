namespace Beatrice.Request
{
    public class ActionRequest
    {
        public string RequestId { get; set; }
        public IntentRequest[] Inputs { get; set; }
    }
}
