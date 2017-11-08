using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beatrice.Configuration
{
    public class DeviceDefinition
    {
        public static readonly string[] KnownDeviceTypes = new[]
        {
            "action.devices.types.CAMERA",
            "action.devices.types.DISHWASHER",
            "action.devices.types.DRYER",
            "action.devices.types.LIGHT",
            "action.devices.types.OUTLET",
            "action.devices.types.SCENE",
            "action.devices.types.SWITCH",
            "action.devices.types.THERMOSTAT",
            "action.devices.types.VACUUM",
            "action.devices.types.WASHER",
        };

        public string Id { get; set; }
        public string Name { get; set; }
        public string[] Nicknames { get; set; }
        public string RoomHint { get; set; }
        public string Type { get; set; }
        public FeatureDefinition[] Features { get; set; }
    }
}
