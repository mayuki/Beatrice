using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beatrice.Web.Models.Configuration
{
    public class BeatriceSecurityConfiguration
    {
        public string User { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Get or set a API key for Google device sync request API.
        /// </summary>
        public string SyncRequestApiKey { get; set; }

        public BeatriceSecurityOAuthConfiguration OAuth { get; set; }
    }

    public class BeatriceSecurityOAuthConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] RedirectUrls { get; set; }
        public bool AllowInsecureHttp { get; set; }
    }
}
