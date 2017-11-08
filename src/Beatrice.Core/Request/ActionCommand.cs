using System;
using System.Collections.Generic;
using System.Linq;

namespace Beatrice.Request
{
    public class ActionCommand
    {
        public static readonly IReadOnlyDictionary<string, Type> ByCommand = GetActionCommandsByCommand();
        public static readonly IReadOnlyDictionary<Type, string> ByType = GetActionCommandsByType();

        public string Command { get; set; }

        [ActionCommand("action.devices.commands.BrightnessAbsolute")]
        public class BrightnessAbsolute : ActionCommand
        {
            public int Brightness { get; set; }
        }

        [ActionCommand("action.devices.commands.SetModes")]
        public class SetModes : ActionCommand
        {
            public string UpdateModeSettings { get; set; }
        }

        [ActionCommand("action.devices.commands.OnOff")]
        public class OnOff : ActionCommand
        {
            public bool On { get; set; }
        }

        [ActionCommand("action.devices.commands.GetCameraStream")]
        public class GetCameraStream : ActionCommand
        {
            public bool StreamToChromecast { get; set; }
            public string[] SupportedStreamProtocols { get; set; }
        }

        [ActionCommand("action.devices.commands.ColorAbsolute")]
        public class ColorAbsolute : ActionCommand
        {
            public string Name { get; set; }
            public int? SpectrumRGB { get; set; }
            public int? Temperature { get; set; }
        }

        [ActionCommand("action.devices.commands.Dock")]
        public class Dock : ActionCommand
        {
        }

        [ActionCommand("action.devices.commands.ActivateScene")]
        public class ActivateScene : ActionCommand
        {
            public bool Deactivate { get; set; }
        }

        [ActionCommand("action.devices.commands.StartStop")]
        public class StartStop : ActionCommand
        {
            public bool Start { get; set; }
        }

        [ActionCommand("action.devices.commands.PauseUnpause")]
        public class PauseUnpause : ActionCommand
        {
            public bool Pause { get; set; }
        }

        [ActionCommand("action.devices.commands.ThermostatTemperatureSetpoint")]
        public class ThermostatTemperatureSetPoint : ActionCommand
        {
            public float ThermostatTemperatureSetpoint { get; set; }
        }

        [ActionCommand("action.devices.commands.ThermostatTemperatureSetRange")]
        public class ThermostatTemperatureSetRange : ActionCommand
        {
            public float ThermostatTemperatureSetpointHigh { get; set; }
            public float ThermostatTemperatureSetpointLow { get; set; }
        }

        [ActionCommand("action.devices.commands.ThermostatSetMode")]
        public class ThermostatSetMode : ActionCommand
        {
            public string ThermostatMode { get; set; }
        }

        [ActionCommand("action.devices.commands.SetToggles")]
        public class SetToggles : ActionCommand
        {
            public Dictionary<string, bool> UpdateToggleSettings { get; set; }
        }

        public class Generic : ActionCommand
        {
            public Dictionary<string, object> Params { get; set; }
        }

        private static Dictionary<string, Type> GetActionCommandsByCommand()
        {
            return typeof(ActionCommand)
                .GetNestedTypes()
                .Select(x => new { Type = x, ActionCommand = x.GetCustomAttributes(true).OfType<ActionCommandAttribute>().FirstOrDefault() })
                .Where(x => x.ActionCommand != null)
                .ToDictionary(k => k.ActionCommand.Name, v => v.Type);
        }

        private static Dictionary<Type, string> GetActionCommandsByType()
        {
            return typeof(ActionCommand)
                .GetNestedTypes()
                .Select(x => new { Type = x, ActionCommand = x.GetCustomAttributes(true).OfType<ActionCommandAttribute>().FirstOrDefault() })
                .Where(x => x.ActionCommand != null)
                .ToDictionary(k => k.Type, v => v.ActionCommand.Name);
        }
    }
}
