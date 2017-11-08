using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beatrice.Request;

namespace Beatrice.Device.Traits
{
    [DeviceTrait(DeviceTraits.CameraStream)]
    public interface ICameraStreamTrait
        : IActionCommandSupport<ActionCommand.GetCameraStream>
    {
    }
}
