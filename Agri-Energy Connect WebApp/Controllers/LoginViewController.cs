﻿using Agri_Energy_Connect_WebApp.Models;
using Agri_Energy_Connect_WebApp.Workers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Agri_Energy_Connect_WebApp.Controllers
{
    public class LoginViewController : Controller
    {
        private readonly Agri_Energy_Connect_WebApp.Data.Agri_Energy_Connect_WebAppContext _context;

        public LoginViewController(Agri_Energy_Connect_WebApp.Data.Agri_Energy_Connect_WebAppContext context)
        {
            GetSet.UserEmployee = null;
            GetSet.UserFarmer = 0;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

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
