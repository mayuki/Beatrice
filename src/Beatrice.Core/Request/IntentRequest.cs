namespace Beatrice.Request
{
    public class IntentRequest
    {
        public string Intent { get; set; }
        public IPayload Payload { get; set; }
    }
}
