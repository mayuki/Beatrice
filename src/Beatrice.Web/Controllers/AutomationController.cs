using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Beatrice.Service;
using Beatrice.Request;
using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OAuth.Validation;

namespace Beatrice.Web.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationConstants.Schemes.Bearer)]
    public class AutomationController : Controller
    {
        private AutomationService _service;

        public AutomationController(AutomationService automationService)
        {
            _service = automationService;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody]ActionRequest request)
        {
            if (request == null) return NotFound();

            var result = await _service.DispatchAsync(request);

            return (result == null)
                ? (IActionResult)NotFound()
                : (IActionResult)Json(result);
        }
    }
}