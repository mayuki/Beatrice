using Beatrice.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Beatrice.Device
{
    public class DeviceInstanceProvider
    {
        private DeviceFeatureProvider _featureProvider;
        public DeviceInstanceProvider(DeviceFeatureProvider featureProvider)
        {
            _featureProvider = featureProvider;
        }

        public DeviceInstance Create(DeviceDefinition definition)
        {
            return new DeviceInstance(_featureProvider, definition);
        }
    }
}
