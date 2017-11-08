using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Beatrice.Web.ViewModels.Account;
using Beatrice.Web.Models.Configuration;
using Beatrice.Web.Models.UseCase;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Beatrice.Web.Controllers
{
    public class AccountController : Controller
    {
        private BeatriceSecurityConfiguration _securityConfig;

        public AccountController(IOptions<BeatriceSecurityConfiguration> securityConfig)
        {
            _securityConfig = securityConfig.Value;
        }

        public IActionResult SignIn(string returnUrl)
        {
            if (String.IsNullOrWhiteSpace(_securityConfig.User) || String.IsNullOrWhiteSpace(_securityConfig.Password))
            {
                throw new Exception("'User' or 'Password' setting must be configured and are not be null.");
            }

            return View(new SignInViewModel
            {
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInFormModel input, [FromServices]ValidateUser useCaseValidateUser)
        {
            if (!ModelState.IsValid)
            {
                return View(new SignInViewModel
                {
                    ReturnUrl = input.ReturnUrl,
                });
            }

            var results = await useCaseValidateUser.ExecuteAsync(input);
            if (results.Any())
            {
                foreach (var error in results)
                {
                    ModelState.AddModelError(error.Key, error.Message);
                }
                return View(new SignInViewModel
                {
                    ReturnUrl = input.ReturnUrl,
                });
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaims(new[]
            {
                new Claim(ClaimTypes.Name, _securityConfig.User),
                new Claim(ClaimTypes.NameIdentifier, _securityConfig.User)
            });

            await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

            return Redirect(input.ReturnUrl);
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
