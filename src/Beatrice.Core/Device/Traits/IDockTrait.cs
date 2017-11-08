using Beatrice.Request;

namespace Beatrice.Device.Traits
{
    [DeviceTrait(DeviceTraits.Dock)]
    public interface IDockTrait
    : IActionCommandSupport<ActionCommand.Dock>
    {
    }
}
