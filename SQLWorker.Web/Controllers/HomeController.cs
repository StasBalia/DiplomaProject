using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLWorker.Web.Models;

namespace SQLWorker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _log;
        public HomeController(ILogger<HomeController> log)
        {
            _log = log;
        }
        public IActionResult Index()
        {
            _log.LogInformation("Hi");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}