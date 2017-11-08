using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beatrice.Device
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class DeviceTraitAttribute : Attribute
    {
        public string Name { get; }
        public DeviceTraitAttribute(string name)
        {
            Name = name;
        }
    }
}
