using Beatrice.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beatrice.Web.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public IReadOnlyDictionary<string, DeviceInstance> DeviceById { get; set; }
    }
}
