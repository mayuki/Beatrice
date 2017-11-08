using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beatrice.Web.Models.Configuration;

namespace Beatrice.Web.Infrastracture
{
    public class BeatriceOpenIdConnectServerProvider : OpenIdConnectServerProvider
    {
        private BeatriceSecurityConfiguration _beatriceSecurityConfiguration;

        public BeatriceOpenIdConnectServerProvider(IOptions<BeatriceSecurityConfiguration> beatriceSecurityConfiguration)
        {
            _beatriceSecurityConfiguration = beatriceSecurityConfiguration.Value;
        }

        public override Task ValidateAuthorizationRequest(ValidateAuthorizationRequestContext context)
        {
            // ref: https://github.com/aspnet-contrib/AspNet.Security.OpenIdConnect.Server/blob/dev/samples/Mvc/Mvc.Server/Providers/AuthorizationProvider.cs
            // Note: to support custom response modes, the OpenID Connect server middleware doesn't
            // reject unknown modes before the ApplyAuthorizationResponse event is invoked.
            // To ensure invalid modes are rejected early enough, a check is made here.
            if (!string.IsNullOrEmpty(context.Request.ResponseMode) && !context.Request.IsFormPostResponseMode() &&
                                                                        !context.Request.IsFragmentResponseMode() &&
                                                                        !context.Request.IsQueryResponseMode())
            {
                context.Reject(
                    error: OpenIdConnectConstants.Errors.InvalidRequest,
                    description: "The specified 'response_mode' is unsupported.");

                return Task.CompletedTask;
            }

            // Retrieve the application details corresponding to the requested client_id.
            if (!String.Equals(context.ClientId, _beatriceSecurityConfiguration.OAuth.ClientId, StringComparison.Ordinal))
            {
                context.Reject(
                    error: OpenIdConnectConstants.Errors.InvalidClient,
                    description: "The specified client identifier is invalid.");

                return Task.CompletedTask;
            }

            if (!String.IsNullOrEmpty(context.RedirectUri) &&
                !_beatriceSecurityConfiguration.OAuth.RedirectUrls.Any(x => x.StartsWith(context.RedirectUri)))
            {
                context.Reject(
                    error: OpenIdConnectConstants.Errors.InvalidClient,
                    description: "The specified 'redirect_uri' is invalid.");

                return Task.CompletedTask;
            }

            context.Validate(context.RedirectUri);
            return Task.CompletedTask;
        }

        public override Task ValidateTokenRequest(ValidateTokenRequestContext context)
        {
            // ref: https://github.com/aspnet-contrib/AspNet.Security.OpenIdConnect.Server/blob/dev/README.md
            if (String.Equals(context.ClientId, _beatriceSecurityConfiguration.OAuth.ClientId, StringComparison.Ordinal) &&
                String.Equals(context.ClientSecret, _beatriceSecurityConfiguration.OAuth.ClientSecret, StringComparison.Ordinal))
            {
                context.Validate();
            }

            return Task.CompletedTask;
        }

        public override Task ProcessSigninResponse(ProcessSigninResponseContext context)
        {
            context.IncludeRefreshToken = true;
            return Task.CompletedTask;
        }
    }
}
