using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Beatrice.Device.Traits;
using Beatrice.Request;
using Microsoft.Extensions.Logging;

namespace Beatrice.Device.Features
{
    public class ExternalProcessOnOff : IOnOffTrait
    {
        private ExternalProcessOnOffOptions _options;
        private ILogger _logger;

        public ExternalProcessOnOff(ExternalProcessOnOffOptions options, ILogger<ExternalProcessOnOff> logger)
        {
            _options = options;
            _logger = logger;
        }

        public Task InvokeAsync(DeviceFeatureInvocationContext ctx, ActionCommand.OnOff commandParams)
        {
            var processConfig = commandParams.On ? _options.On : _options.Off;
            _logger.LogInformation($"ExternalProcessOnOff: Executable={processConfig.Executable}; Arguments={processConfig.Arguments}");

            var process = Process.Start(processConfig.Executable, processConfig.Arguments);
            if (processConfig.WaitForExit)
            {
                process.WaitForExit();
            }

            return Task.CompletedTask;
        }
    }

    public class ExternalProcessOnOffOptions : IDeviceFeatureOption
    {
        public ProcessConfig On { get; set; }
        public ProcessConfig Off { get; set; }

        public class ProcessConfig
        {
            public string Executable { get; set; }
            public string Arguments { get; set; }
            public bool WaitForExit { get; set; }
        }
    }
}
