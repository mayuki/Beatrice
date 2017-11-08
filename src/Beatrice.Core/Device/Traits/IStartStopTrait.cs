using Beatrice.Request;

namespace Beatrice.Device.Traits
{
    [DeviceTrait(DeviceTraits.StartStop)]
    public interface IStartStopTrait
    : IActionCommandSupport<ActionCommand.StartStop>
    , IActionCommandSupport<ActionCommand.PauseUnpause>
    {
    }
}
