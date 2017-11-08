using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beatrice.Request;
using Microsoft.Extensions.Configuration;

namespace Beatrice.Device
{
    public class DeviceFeature
    {
        public object Instance { get; }

        public string[] Commands { get; }
        public string[] Traits { get; }

        public DeviceFeature(object instance)
        {
            Instance = instance;
            Commands = DeviceTraitHelper.GetSupportedActionCommands(Instance);
            Traits = DeviceTraitHelper.GetSupportedTraits(Instance);
        }

        public Task InvokeAsync(DeviceFeatureInvocationContext ctx, ActionCommand actionCommand)
        {
            return DeviceFeatureHelper.InvokeAsync(this, ctx, actionCommand);
        }
    }
}
