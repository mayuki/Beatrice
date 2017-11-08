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
    public class LoggingOnOff : IOnOffTrait
    {
        private LoggingOnOffOptions _options;
        private ILogger _logger;

        public LoggingOnOff(LoggingOnOffOptions options, ILogger<LoggingOnOff> logger)
        {
            _options = options;
            _logger = logger;
        }

        public Task InvokeAsync(DeviceFeatureInvocationContext ctx, ActionCommand.OnOff commandParams)
        {
            var message = commandParams.On ? _options.On : _options.Off;
            _logger.LogInformation(message);

            return Task.CompletedTask;
        }
    }

    public class LoggingOnOffOptions : IDeviceFeatureOption
    {
        public string On { get; set; }
        public string Off { get; set; }
    }
}
