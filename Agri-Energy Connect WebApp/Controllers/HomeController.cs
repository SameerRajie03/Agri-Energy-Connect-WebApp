using Agri_Energy_Connect_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Agri_Energy_Connect_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                return loginCheckResult;
            }
            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                return loginCheckResult;
            }
            else
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                return loginCheckResult;
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
