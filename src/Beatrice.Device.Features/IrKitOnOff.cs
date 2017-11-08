using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beatrice.Device.Traits;
using Beatrice.Request;
using Beatrice.Device.Features.Internal;
using Microsoft.Extensions.Logging;

namespace Beatrice.Device.Features
{
    public class IrKitOnOff : IOnOffTrait
    {
        private IrKitOnOffOptions _options;
        private ILogger _logger;

        public IrKitOnOff(IrKitOnOffOptions options, ILogger<IrKitOnOff> logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task InvokeAsync(DeviceFeatureInvocationContext ctx, ActionCommand.OnOff commandParams)
        {
            _logger.LogInformation("IrKitSimpleOnOff: " + commandParams.On);
            await new IrKitClient(_options.EndPoint).SendMessagesAsync(commandParams.On ? _options.On : _options.Off);
        }
    }

    public class IrKitOnOffOptions : IDeviceFeatureOption
    {
        public string On { get; set; }
        public string Off { get; set; }
        public string EndPoint { get; set; }
    }
}
