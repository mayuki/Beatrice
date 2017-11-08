using Beatrice.Request;

namespace Beatrice.Device.Traits
{
    [DeviceTrait(DeviceTraits.ColorTemperature)]
    public interface IColorTemperatureTrait
    : IActionCommandSupport<ActionCommand.ColorAbsolute>
    {
    }
}
