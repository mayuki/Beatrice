using Beatrice.Request;

namespace Beatrice.Device.Traits
{
    [DeviceTrait(DeviceTraits.Modes)]
    public interface IModesTrait
    : IActionCommandSupport<ActionCommand.SetModes>
    {
    }
}
