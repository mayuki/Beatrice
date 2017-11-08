using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Beatrice.Device.Features.Internal
{
    public class IrKitClient
    {
        private HttpClient _client;
        private string _hostName;

        public IrKitClient(string hostName)
        {
            this._hostName = hostName;
            this._client = new HttpClient();
            this._client.DefaultRequestHeaders.Add("X-Requested-With", "IrKitClient");
        }

        public async Task SendMessagesAsync(string value)
        {
            await this._client.PostAsync(
                $"http://{this._hostName}/messages",
                new ByteArrayContent(new UTF8Encoding(false).GetBytes(value))
            ).ConfigureAwait(false);
        }

        public Task<string> GetMessagesAsync()
        {
            return this.GetMessagesAsync(
                waitUntilReceived: true,
                cancellationToken: CancellationToken.None
            );
        }

        public Task<string> GetMessagesAsync(CancellationToken cancellationToken)
        {
            return this.GetMessagesAsync(
                waitUntilReceived: true,
                cancellationToken: cancellationToken
            );
        }

        public async Task<string> GetMessagesAsync(bool waitUntilReceived, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = await this._client.GetStringAsync($"http://{this._hostName}/messages").ConfigureAwait(false);
                if (!String.IsNullOrWhiteSpace(result))
                {
                    return result;
                }

                if (!waitUntilReceived) return null;
            }

            return null;
        }

        //public async Task<IrKitClientKey> GetKeys()
        //{
        //    return null;
        //}
    }

    public class IrKitClientKey
    {
        public string ClientToken { get; set; }
        public string DeviceId { get; set; }
    }
}
