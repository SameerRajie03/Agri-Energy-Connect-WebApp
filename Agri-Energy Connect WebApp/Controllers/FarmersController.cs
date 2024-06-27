using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect_WebApp.Data;
using Agri_Energy_Connect_WebApp.Models;
using Agri_Energy_Connect_WebApp.Workers;

namespace Agri_Energy_Connect_WebApp.Controllers
{
    public class FarmersController : Controller
    {
        /// <summary>
        /// when any class/view calls for this controller, the app's context is parsed to a variable in the constructer.
        /// the value of that variable is them assigned to a read only variable that is globally assigned for the class.
        /// </summary>
        private readonly Agri_Energy_Connect_WebAppContext _context;

        public FarmersController(Agri_Energy_Connect_WebAppContext context)
        {
            _context = context;
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// All get classes first checks if there is a user that is logged into the application for added security.
        /// If a user who isn't logged in tries to access those pages, they will be redirected to the login page with an error notification.
        /// If a user who doesn't have access to the page tries to access the page, they will be redirected to their respective home page.
        /// </summary>
        //---------------------------------------------------------------------------------------------------------------------------------
        // GET: Farmers
        public async Task<IActionResult> Index()
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                return View(await _context.Farmer.ToListAsync());
            }

        }
        //---------------------------------------------------------------------------------------------------------------------------------
        // GET: Farmers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var farmer = await _context.Farmer
                    .FirstOrDefaultAsync(m => m.FarmerId == id);
                if (farmer == null)
                {
                    return NotFound();
                }
                return View(farmer);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Loads Page for employee to create a new farmer
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                if(Validation.EmployeeExistsId(GetSet.UserEmployee, _context))
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
        /// Code for Employee to create a new farmer
        /// </summary>
        /// <param name="farmer"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FarmerId,Name,Surname,Username,Password")] Farmer farmer)
        {
            farmer.Username = farmer.Name + farmer.Surname;
            if (ModelState.IsValid)
            {
                //IDs auto iterate so that when a new Farmer is added
                _context.Add(farmer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Farmer Added";
                return RedirectToAction("IndexEmployee", "Home");
            }
            return View(farmer);
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Loads sign up page for the farmers
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmer = await _context.Farmer.FindAsync(id);
            if (farmer == null)
            {
                return NotFound();
            }
            return View(farmer);
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Lets the user who wants to login as a farmer create/change their username/password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="farmer"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FarmerId,Name,Surname,Username,Password")] Farmer farmer)
        {
            if (id != farmer.FarmerId)
            {
                return NotFound();
            }
 
            if (ModelState.IsValid)
            {
                if (Validation.ValidPassword(farmer.Password))
                {
                        try
                        {
                            farmer.Password = DataTypeChange.ToHash(farmer.Password);
                            _context.Update(farmer);
                            await _context.SaveChangesAsync();
                            TempData["SuccessMessage"] = "Username and Password Saved";
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!FarmerExists(farmer.FarmerId))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                    return RedirectToAction("Login", "LoginView");
                }
                else
                {
                    ModelState.AddModelError("Employee.Password", "Invalid Password\nPassword value must meet the following requirements: " +
                        "\n- Be at least 8 characters long" +
                        "\n- Contain no empty spaces" +
                        "\n- Contain at least one capital letter" +
                        "\n- Contain at least one number");
                }
            }
            return View(farmer);
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        // GET: Farmers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                TempData["Login"] = "You need to Login First";
                return loginCheckResult;
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var farmer = await _context.Farmer
                    .FirstOrDefaultAsync(m => m.FarmerId == id);
                if (farmer == null)
                {
                    return NotFound();
                }

                return View(farmer);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        // POST: Farmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var farmer = await _context.Farmer.FindAsync(id);
            if (farmer != null)
            {
                _context.Farmer.Remove(farmer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks if the selected farmer exeists in the context of the application
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool FarmerExists(int id)
        {
            return _context.Farmer.Any(e => e.FarmerId == id);
        }
        //---------------------------------------------------------------------------------------------------------------------------------
    }
}
//--------------------------------------------------End of Code------------------------------------------------------------