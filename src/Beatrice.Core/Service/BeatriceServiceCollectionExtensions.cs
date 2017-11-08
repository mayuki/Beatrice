using Beatrice.Configuration;
using Beatrice.Device;
using Beatrice.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BeatriceServiceCollectionExtensions
    {
        public static void AddBeatrice(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DeviceConfiguration>(configuration);
            services.AddSingleton<DeviceInstanceProvider>();
            services.AddSingleton<DeviceFeatureProvider>();
            services.AddSingleton<AutomationService>();

            FeatureLoader.Register(services, Array.Empty<string>());
        }
    }
}
