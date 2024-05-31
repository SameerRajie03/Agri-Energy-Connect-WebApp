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
        private readonly Agri_Energy_Connect_WebAppContext _context;

        public FarmersController(Agri_Energy_Connect_WebAppContext context)
        {
            _context = context;
        }

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

        // GET: Farmers/Create
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

        // POST: Farmers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FarmerId,Name,Surname,Username,Password")] Farmer farmer)
        {
            farmer.Username = farmer.Name + farmer.Surname;
            if (ModelState.IsValid)
            {
                
                _context.Add(farmer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Farmer Added";
                return RedirectToAction("IndexEmployee", "Home");
            }
            return View(farmer);
        }

        // GET: Farmers/Edit/5
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

        // POST: Farmers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        private bool FarmerExists(int id)
        {
            return _context.Farmer.Any(e => e.FarmerId == id);
        }
    }
}
