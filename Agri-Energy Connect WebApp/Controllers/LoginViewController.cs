using Agri_Energy_Connect_WebApp.Models;
using Agri_Energy_Connect_WebApp.Workers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Agri_Energy_Connect_WebApp.Controllers
{
    public class LoginViewController : Controller
    {
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// when any class/view calls for this controller, the app's context is parsed to a variable in the constructer.
        /// the value of that variable is them assigned to a read only variable that is globally assigned for the class.
        /// 
        /// The Globally set variables to save the logged in users are reset to log them out of the app
        /// </summary>
        private readonly Agri_Energy_Connect_WebApp.Data.Agri_Energy_Connect_WebAppContext _context;

        public LoginViewController(Agri_Energy_Connect_WebApp.Data.Agri_Energy_Connect_WebAppContext context)
        {
            GetSet.UserEmployee = null;
            GetSet.UserFarmer = 0;
            _context = context;
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Loads the login page
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// calls methods to check if the login details are valid.
        /// if they are valid it calls the methods to log the user in either as an employee or a farmer.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            bool isAuthenticated = false;
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (Validation.UsernameExists(model.Username, _context))
            {
                if (Validation.FarmerExists(model.Username, _context))
                {
                    isAuthenticated = AuthenticateFarmer(model.Username, model.Password);
                    if (isAuthenticated)
                    {
                        TempData["SuccessMessage"] = model.Username + " Logged In";
                        return RedirectToAction("IndexFarmer", "Home"); 
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View();
                    }
                }
                if (Validation.EmployeeExists(model.Username, _context))
                {
                    isAuthenticated = AuthenticateEmployee(model.Username, model.Password);
                    if (isAuthenticated)
                    {
                        TempData["SuccessMessage"] = model.Username + " Logged In";
                        return RedirectToAction("IndexEmployee", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                return View();
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// if the details to login matches that of an employee, the user logs in as an Employee.
        /// That redirects them to the relevent home page
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// if the details to login matches that of a farmer, the user logs in as a Farmer.
        /// That redirects them to the relevent home page
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
    }
}
//--------------------------------------------------End of Code------------------------------------------------------------
