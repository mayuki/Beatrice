using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Beatrice.Web.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Beatrice.Request;
using Beatrice.Configuration;
using Beatrice.Service;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Beatrice.Web.Models.Configuration;
using Beatrice.Web.Models.UseCase;
using Beatrice.Web.ViewModels.Home;

namespace Beatrice.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index([FromServices]AutomationService automationService)
        {
            return View(new HomeIndexViewModel
            {
                DeviceById = automationService.DeviceById,
            });
        }

        [Authorize]
        public async Task<IActionResult> Resync([FromServices]Resync useCaseResync)
        {
            await useCaseResync.ExecuteAsync();

            return RedirectToAction("Index");
        }
    }
}
