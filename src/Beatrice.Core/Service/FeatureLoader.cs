using Beatrice.Device;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Beatrice.Service
{
    public static class FeatureLoader
    {
        private static readonly string[] PredefinedAssemblies = new[]
        {
            "Beatrice.Device.Features"
        };

        public static void Register(IServiceCollection services, string[] assemblies)
        {
            foreach (var asmName in PredefinedAssemblies.Concat(assemblies))
            {
                var asm = Assembly.Load(asmName);

                foreach (var t in asm.GetTypes().Where(x => IsFeature(x)))
                {
                    //services.AddTransient(t);
                }
            }
        }

        private static bool IsFeature(Type t)
        {
            return t.GetInterfaces()
                .Where(x => x.IsGenericType)
                .Select(x => x.GetGenericTypeDefinition())
                .Any(x => x == typeof(IActionCommandSupport<>));
        }
    }
}
