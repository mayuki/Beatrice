using System.Collections.Generic;

namespace Beatrice.Response
{
    public class QueryActionResponse : ActionResponse<QueryActionResponse.QueryActionPayload>
    {
        public class QueryActionPayload
        {
            public Dictionary<string, object>[] Devices { get; set; }
        }
    }
}
