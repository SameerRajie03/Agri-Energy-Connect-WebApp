using Agri_Energy_Connect_WebApp.Models;
using Agri_Energy_Connect_WebApp.Workers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Agri_Energy_Connect_WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly Agri_Energy_Connect_WebApp.Data.Agri_Energy_Connect_WebAppContext _context;

        public LoginController(Agri_Energy_Connect_WebApp.Data.Agri_Energy_Connect_WebAppContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            Validation val = new Validation();
            bool isAuthenticated = false;
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!val.UsernameExists(model.Username, _context))
            {
                if(!val.FarmerExists(model.Username, _context))
                {
                    isAuthenticated = AuthenticateFarmer(model.Username, model.Password);
                }
                if(!val.EmployeeExists(model.Username, _context))
                {
                    isAuthenticated = AuthenticateEmployee(model.Username, model.Password);
                }

                if (!isAuthenticated)
                {
                    ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                    return View();
                }
                else
                {
                    return RedirectToRoute("/Home/Index");
                }

            }
            else
            {

            }
            
            
        }


        private bool AuthenticateEmployee(string username, string password)
        {
            // Add your logic to check the username and password against your user database
            Employee? user = _context.Employee.Where(u => u.Username == username).FirstOrDefault();
            if (user != null)
            {
                string hashed = DataTypeChange.ToHash(password);

                if (hashed == user.Password)
                {
                    GetSet.UserEmployee = user.EmployeeId;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool AuthenticateFarmer(string username, string password)
        {
            // Add your logic to check the username and password against your user database
            Farmer? user = _context.Farmer.Where(u => u.Username == username).FirstOrDefault();
            if (user != null)
            {
                string hashed = DataTypeChange.ToHash(password);

                if (hashed == user.Password)
                {
                    GetSet.UserFarmer = user.FarmerId;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
