using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beatrice.Device
{
    public class DeviceFeatureProvider
    {
        private IServiceProvider _serviceProvider;

        public DeviceFeatureProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public DeviceFeature Create(string typeName, IConfigurationSection options)
        {
            var t = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetType(typeName)).FirstOrDefault(x => x != null);
            if (t == null)
            {
                throw new ArgumentException(String.Format("DeviceFeature '{0}' is not supported.", typeName), nameof(typeName));
            }

            var parameters = t.GetConstructors().First().GetParameters();
            if (parameters.Length == 0)
            {
                return new DeviceFeature(Activator.CreateInstance(t));
            }
            else
            {
                var args = parameters.Select(x =>
                {
                    if (typeof(IDeviceFeatureOption).IsAssignableFrom(x.ParameterType))
                    {
                        return options.Get(x.ParameterType);
                    }
                    else
                    {
                        return _serviceProvider.GetService(x.ParameterType);
                    }
                }).ToArray();

                return new DeviceFeature(Activator.CreateInstance(t, args));
            }
        }
    }
}
