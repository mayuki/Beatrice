using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beatrice.Request;

namespace Beatrice.Device
{
    public static class DeviceTraitHelper
    {
        public static string[] GetSupportedActionCommands(object o)
        {
            return GetSupportedActionCommands(o.GetType());
        }
        public static string[] GetSupportedActionCommands(Type t)
        {
            var supportedActionInterfaces = t.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IActionCommandSupport<>));
            return supportedActionInterfaces.Select(x => ActionCommand.ByType[x.GetGenericArguments()[0]]).ToArray();
        }

        public static string[] GetSupportedTraits(object o)
        {
            return GetSupportedTraits(o.GetType());
        }
        public static string[] GetSupportedTraits(Type t)
        {
            return t.GetInterfaces()
                .SelectMany(x => x.GetCustomAttributes(true))
                .OfType<DeviceTraitAttribute>()
                .Select(x => x.Name)
                .ToArray();
        }
    }
}
