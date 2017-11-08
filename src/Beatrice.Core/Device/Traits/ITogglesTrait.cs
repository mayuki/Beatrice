using Beatrice.Request;

namespace Beatrice.Device.Traits
{
    [DeviceTrait(DeviceTraits.Toggles)]
    public interface ITogglesTrait
    : IActionCommandSupport<ActionCommand.SetToggles>
    {
    }
}
