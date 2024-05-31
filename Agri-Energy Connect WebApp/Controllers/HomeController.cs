using Agri_Energy_Connect_WebApp.Models;
using Agri_Energy_Connect_WebApp.Workers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Agri_Energy_Connect_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Agri_Energy_Connect_WebApp.Data.Agri_Energy_Connect_WebAppContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Data.Agri_Energy_Connect_WebAppContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                return View();
            }
        }

        public IActionResult IndexEmployee()
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                if (Validation.EmployeeExistsId(GetSet.UserEmployee, _context))
                {
                    return View();
                }
                else
                {
                    TempData["ErrorMessage"] = "You do not have access to this page";
                    return RedirectToAction("IndexFarmer", "Home");
                }
            }
        }

        public IActionResult IndexFarmer()
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                if (Validation.FarmerExistsId(GetSet.UserFarmer.ToString(), _context))
                {
                    return View();
                }
                else
                {
                    TempData["ErrorMessage"] = "You do not have access to this page";
                    return RedirectToAction("IndexEmployee", "Home");
                }
            }
        }

        public IActionResult Privacy()
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
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
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}
