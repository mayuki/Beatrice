using Beatrice.Request;

namespace Beatrice.Device.Traits
{
    [DeviceTrait(DeviceTraits.Scene)]
    public interface ISceneTrait
    : IActionCommandSupport<ActionCommand.ActivateScene>
    {
    }
}
