namespace Beatrice.Response
{
    public class SyncActionResponse : ActionResponse<SyncActionResponse.SyncActionPayload>
    {
        public class SyncActionPayload
        {
            public string AgentUserId { get; set; }
            public DeviceResponse[] Devices { get; set; }
        }

        public class DeviceResponse
        {
            public string Id { get; set; }
            public NameResponse Name { get; set; }
            public string Type { get; set; }
            public string[] Traits { get; set; }
            public bool WillReportState { get; set; }
            public string RoomHint { get; set; }
        }

        public class NameResponse
        {
            public string Name { get; set; }
            public string[] Nicknames { get; set; }
            public string[] DefaultNames { get; set; }
        }
    }
}
