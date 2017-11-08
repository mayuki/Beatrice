using Beatrice.Service;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Beatrice.Web.Models.Configuration;

namespace Beatrice.Web.Models.UseCase
{
    public class Resync
    {
        private string _apiKey;
        private string _agentUserId;

        public Resync(IOptions<BeatriceSecurityConfiguration> securityConfiguration, AutomationService automationService)
        {
            _apiKey = securityConfiguration.Value.SyncRequestApiKey;
            _agentUserId = automationService.AgentUserId; // TODO:
        }

        public Task ExecuteAsync()
        {
            var url = $"https://homegraph.googleapis.com/v1/devices:requestSync?key={_apiKey}";
            var content = new StringContent($"{{agent_user_id: \"{ _agentUserId}\"}}", new UTF8Encoding(false), "application/json");
            return new HttpClient().PostAsync(url, content);
        }
    }
}
