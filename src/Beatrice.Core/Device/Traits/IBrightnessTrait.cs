using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beatrice.Request;

namespace Beatrice.Device.Traits
{
    [DeviceTrait(DeviceTraits.Brightness)]
    public interface IBrightnessTrait
        : IActionCommandSupport<ActionCommand.BrightnessAbsolute>
    {
    }
}
