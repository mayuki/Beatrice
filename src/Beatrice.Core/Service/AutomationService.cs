using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beatrice.Configuration;
using Beatrice.Device;
using Beatrice.Request;
using Beatrice.Response;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Beatrice.Service
{
    public class AutomationService
    {
        private Dictionary<string, DeviceInstance> _deviceById;
        private ILogger _logger;

        public string AgentUserId { get; set; } = "agentUserId.0";
        public IReadOnlyDictionary<string, DeviceInstance> DeviceById => _deviceById;

        public AutomationService(IOptions<DeviceConfiguration> deviceConfig, DeviceInstanceProvider deviceInstanceProvider, ILogger<AutomationService> logger)
        {
            _deviceById = deviceConfig.Value.Devices.ToDictionary(k => k.Id, v => deviceInstanceProvider.Create(v));
            _logger = logger;

            foreach (var device in _deviceById)
            {
                _logger.LogInformation("Device: Id={0}; Features={1}", device.Key, String.Join(",", device.Value.Features.Select(x => x.Instance.GetType().ToString())));
            }
        }

        public async Task<object> DispatchAsync(ActionRequest request)
        {
            foreach (var intent in request.Inputs)
            {
                switch (intent.Intent)
                {
                    case "action.devices.SYNC":
                        return await SyncAsync(request.RequestId);
                    case "action.devices.QUERY":
                        return await GetStatesAsync(request.RequestId, (StatesPayload)intent.Payload);
                    case "action.devices.EXECUTE":
                        return await ExecuteAsync(request.RequestId, (ExecutePayload)intent.Payload);
                }
            }
            return null;
        }

        public async Task<ExecuteActionResponse> ExecuteAsync(string requestId, ExecutePayload executePayload)
        {
            var commandResponses = new List<CommandResult>();
            foreach (var command in executePayload.Commands)
            {
                var successIds = new HashSet<string>();
                var errorIdsByCode = new Dictionary<string, HashSet<string>>();

                foreach (var device in command.Devices)
                {
                    foreach (var exec in command.Execution)
                    {
                        var errorCode = (string)null;

                        if (_deviceById.TryGetValue(device.Id, out var deviceImpl))
                        {
                            try
                            {
                                _logger.LogInformation("Begin Execute: Device={0}({1}); Command={2}", deviceImpl.Definition.Name, deviceImpl.Definition.Id, exec.Command);
                                await deviceImpl.InvokeAsync(exec);
                                successIds.Add(device.Id);
                            }
                            catch
                            {
                                errorCode = String.Empty; // default errorCode
                            }
                        }
                        else
                        {
                            errorCode = String.Empty; // default errorCode
                        }
                        _logger.LogInformation("End Execute: Device={0}({1}); Command={2}; ErrorCode={3}", deviceImpl.Definition.Name, deviceImpl.Definition.Id, exec.Command, errorCode);

                        if (errorCode != null)
                        {
                            if (!errorIdsByCode.ContainsKey(errorCode)) errorIdsByCode[errorCode] = new HashSet<string>();
                            errorIdsByCode[errorCode].Add(device.Id);
                        }
                    }
                }

                if (successIds.Any())
                {
                    commandResponses.Add(new CommandResult
                    {
                        Ids = successIds.ToArray(),
                        Status = "SUCCESS",
                        //States = new Dictionary<string, object>
                        //{
                        //    { "on", true },
                        //    { "online", true },
                        //}
                    });
                }

                foreach (var errorCode in errorIdsByCode.Keys)
                {
                    if (errorIdsByCode[errorCode].Any())
                    {
                        commandResponses.Add(new CommandResult
                        {
                            Ids = successIds.ToArray(),
                            Status = "ERROR",
                            ErrorCode = errorCode,
                        });
                    }
                }
            }

            return new ExecuteActionResponse
            {
                RequestId = requestId,
                Payload = new ExecuteActionResponse.ExecuteActionPayload
                {
                    Commands = commandResponses.ToArray()
                }
            };
        }

        public async Task<QueryActionResponse> GetStatesAsync(string requestId, StatesPayload statesPayload)
        {
            return new QueryActionResponse
            {
                RequestId = requestId,
                Payload = new QueryActionResponse.QueryActionPayload
                {
                    Devices = await Task.WhenAll(statesPayload.Devices.Select(async x => await GetStateFromDeviceIdAsync(x.Id)))
                }
            };
        }
        private async Task<Dictionary<string, object>> GetStateFromDeviceIdAsync(string id)
        {
            var state = new Dictionary<string, object>();
            if (_deviceById[id].WillReportState)
            {
                //foreach (var support in _deviceById[id].Features.OfType<IDeviceSupportWithState>())
                //{
                //    var state2 = await support.GetStateAsync(_deviceById[id].Definition);
                //    foreach (var keyValue in state2)
                //    {
                //        state[keyValue.Key] = keyValue.Value;
                //    }
                //}
            }

            return state;
        }

        public Task<SyncActionResponse> SyncAsync(string requestId)
        {
            _logger.LogInformation("Sync: RequestId={0}", requestId);
            return Task.FromResult(new SyncActionResponse
            {
                RequestId = requestId,
                Payload = new SyncActionResponse.SyncActionPayload
                {
                    AgentUserId = AgentUserId,
                    Devices = _deviceById.Select(x =>
                    {
                        return new SyncActionResponse.DeviceResponse
                        {
                            Id = x.Key,
                            Name = new SyncActionResponse.NameResponse
                            {
                                Name = x.Value.Definition.Name,
                                Nicknames = (x.Value.Definition.Nicknames == null || !x.Value.Definition.Nicknames.Any()) 
                                    ? new[] { x.Value.Definition.Name }
                                    : x.Value.Definition.Nicknames,
                                DefaultNames = new[] { x.Value.Definition.Name },
                            },
                            Type = x.Value.Definition.Type,
                            Traits = x.Value.Traits,
                            WillReportState = x.Value.WillReportState,
                            RoomHint = x.Value.Definition.RoomHint,
                        };
                    }).ToArray()
                }
            });
        }

    }
}
