using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beatrice.Device
{
    public class DeviceFeatureInvocationContext
    {
        public DeviceInstance Device { get; }

        public DeviceFeatureInvocationContext(DeviceInstance device)
        {
            Device = device;
        }
    }
}
