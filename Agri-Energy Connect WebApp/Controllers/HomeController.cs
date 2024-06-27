using Agri_Energy_Connect_WebApp.Models;
using Agri_Energy_Connect_WebApp.Workers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Agri_Energy_Connect_WebApp.Controllers
{
    public class HomeController : Controller
    {
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// when any class/view calls for this controller, the app's context is parsed to a variable in the constructer.
        /// the value of that variable is them assigned to a read only variable that is globally assigned for the class.
        /// </summary>
        private readonly Agri_Energy_Connect_WebApp.Data.Agri_Energy_Connect_WebAppContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, Data.Agri_Energy_Connect_WebAppContext context)
        {
            _logger = logger;
            _context = context;
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// All get classes first checks if there is a user that is logged into the application for added security.
        /// If a user who isn't logged in tries to access those pages, they will be redirected to the login page with an error notification.
        /// If a user who doesn't have access to the page tries to access the page, they will be redirected to their respective home page.
        /// </summary>
        /// //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Loads the home page for users who are not logged into the application
        /// </summary>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Loads the home page for users who are logged in as an Employee
        /// </summary>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Loads the home page for users who are logged in as a Farmer
        /// </summary>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
    }
}
//--------------------------------------------------End of Code------------------------------------------------------------