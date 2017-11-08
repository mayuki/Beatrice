using Beatrice.Request;

namespace Beatrice.Device.Traits
{
    [DeviceTrait(DeviceTraits.ColorSpectrum)]
    public interface IColorSpectrumTrait
        : IActionCommandSupport<ActionCommand.ColorAbsolute>
    {
    }
}
