namespace Beatrice.Response
{
    public class ExecuteActionResponse : ActionResponse<ExecuteActionResponse.ExecuteActionPayload>
    {
        public class ExecuteActionPayload
        {
            public CommandResult[] Commands { get; set; }
        }
    }
}
