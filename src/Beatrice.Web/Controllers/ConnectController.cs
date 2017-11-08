using AspNet.Security.OAuth.Validation;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Beatrice.Web.Controllers
{
    public class ConnectController : Controller
    {
        [Authorize]
        [Route("connect/authorize")]
        public IActionResult Authorize()
        {
            var request = HttpContext.GetOpenIdConnectRequest();
            var response = HttpContext.GetOpenIdConnectResponse();
            if (response != null)
            {
                return View("Error", response);
            }

            var identity = new ClaimsIdentity(
                OpenIdConnectServerDefaults.AuthenticationScheme,
                OpenIdConnectConstants.Claims.Name,
                OpenIdConnectConstants.Claims.Role);

            identity.AddClaim(
                new Claim(OpenIdConnectConstants.Claims.Subject, User.FindFirst(ClaimTypes.NameIdentifier).Value)
                    .SetDestinations(OpenIdConnectConstants.Destinations.AccessToken,
                                        OpenIdConnectConstants.Destinations.IdentityToken));
            identity.AddClaim(
                new Claim(OpenIdConnectConstants.Claims.Name, User.FindFirst(ClaimTypes.Name).Value)
                    .SetDestinations(OpenIdConnectConstants.Destinations.AccessToken,
                                        OpenIdConnectConstants.Destinations.IdentityToken));
            identity.AddClaim(
                new Claim("Beatrice.ClientId", request.ClientId)
                    .SetDestinations(OpenIdConnectConstants.Destinations.AccessToken,
                                        OpenIdConnectConstants.Destinations.IdentityToken));

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), OpenIdConnectServerDefaults.AuthenticationScheme);

            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }
    }
}
