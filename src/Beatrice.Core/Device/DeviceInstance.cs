using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Beatrice.Configuration;
using Beatrice.Request;

namespace Beatrice.Device
{
    public class DeviceInstance
    {
        public DeviceDefinition Definition { get; }
        public DeviceFeature[] Features { get; }

        public string[] Traits { get; }
        public Dictionary<string, DeviceFeature> FeatureByCommand { get; }
        public bool WillReportState { get; }

        public DeviceInstance(DeviceFeatureProvider featureProvider, DeviceDefinition definition)
        {
            Definition = definition;
            Features = definition.Features
                .Select(x => featureProvider.Create(x.Feature, x.Options))
                .ToArray();

            Traits = GetSupportedTraits();
            FeatureByCommand = Features
                .SelectMany(x => x.Commands.Select(y => new { Command = y, Feature = x }))
                .ToDictionary(k => k.Command, v => v.Feature);

            WillReportState = false;
        }

        private string[] GetSupportedTraits()
        {
            var hashSet = new HashSet<string>();
            foreach (var trait in Features.SelectMany(x => x.Traits))
            {
                if (hashSet.Contains(trait)) throw new Exception($"Trait '{trait}' already has been registered.");
                hashSet.Add(trait);
            }

            return hashSet.ToArray();
        }

        public Task InvokeAsync(ActionCommand actionCommand)
        {
            if (actionCommand == null) return Task.CompletedTask;

            if (ActionCommand.ByType.TryGetValue(actionCommand.GetType(), out var command))
            {
                if (FeatureByCommand.TryGetValue(command, out var feature))
                {
                    var ctx = new DeviceFeatureInvocationContext(this);
                    return feature.InvokeAsync(ctx, actionCommand);
                }
            }

            return Task.CompletedTask;
        }
    }
}
