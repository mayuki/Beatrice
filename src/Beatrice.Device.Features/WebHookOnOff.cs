using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Beatrice.Device.Traits;
using Beatrice.Request;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Beatrice.Device.Features
{
    public class WebHookOnOff : IOnOffTrait
    {
        private const string DefaultContentType = "application/json";

        private WebHookOnOffOptions _options;
        private ILogger _logger;

        public WebHookOnOff(WebHookOnOffOptions options, ILogger<WebHookOnOff> logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task InvokeAsync(DeviceFeatureInvocationContext ctx, ActionCommand.OnOff commandParams)
        {
            var endpoint = commandParams.On ? _options.On : _options.Off;
            _logger.LogInformation("WebHookSimpleOnOff: " + endpoint.Url);

            var contentType = String.IsNullOrWhiteSpace(endpoint.ContentType) ? endpoint.ContentType : DefaultContentType;
            await new HttpClient().PostAsync(endpoint.Url, new StringContent(endpoint.Body ?? "", new UTF8Encoding(false), contentType));
        }
    }

    public class WebHookOnOffOptions : IDeviceFeatureOption
    {
        public EndPointConfig On { get; set; }
        public EndPointConfig Off { get; set; }

        public class EndPointConfig
        {
            public string Url { get; set; }
            public string Body { get; set; }
            public string ContentType { get; set; }
        }
    }
}
