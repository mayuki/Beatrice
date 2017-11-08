using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beatrice.Request;

namespace Beatrice.Device.Traits
{
    [DeviceTrait(DeviceTraits.TemperatureSetting)]
    public interface ITemperatureSettingTrait
        : IActionCommandSupport<ActionCommand.ThermostatTemperatureSetPoint>
        , IActionCommandSupport<ActionCommand.ThermostatTemperatureSetRange>
        , IActionCommandSupport<ActionCommand.ThermostatSetMode>
    {
    }
}
