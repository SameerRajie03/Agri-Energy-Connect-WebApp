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
using Microsoft.CodeAnalysis.Operations;

namespace Agri_Energy_Connect_WebApp.Controllers
{
    public class EmployeesController : Controller
    {
        /// <summary>
        /// when any class/view calls for this controller, the app's context is parsed to a variable in the constructer.
        /// the value of that variable is them assigned to a read only variable that is globally assigned for the class.
        /// </summary>
        private readonly Agri_Energy_Connect_WebAppContext _context;

        public EmployeesController(Agri_Energy_Connect_WebAppContext context)
        {
            _context = context;
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// All get classes first checks if there is a user that is logged into the application for added security.
        /// If a user who isn't logged in tries to access those pages, they will be redirected to the login page with an error notification.
        /// If a user who doesn't have access to the page tries to access the page, they will be redirected to their respective home page.
        /// </summary>
        // GET: Employees
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
                return View(await _context.Employee.ToListAsync());
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        // GET: Employees/Details/5
        public async Task<IActionResult> Details(string id)
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

                var employee = await _context.Employee
                    .FirstOrDefaultAsync(m => m.EmployeeId == id);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        // GET: Employees/Create
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
                return View();
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,Name,Surname,Username,Password")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                //IDs auto iterate so that when a new Employee is added
                employee.Username = employee.EmployeeId.ToString();
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Allows the Employee to create their password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EmployeeId,Name,Surname,Username,Password")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (Validation.ValidPassword(employee.Password))
                {
                        try
                        {
                            employee.Password = DataTypeChange.ToHash(employee.Password);
                            _context.Update(employee);
                            await _context.SaveChangesAsync();
                            TempData["SuccessMessage"] = "Username and Password Saved";
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!EmployeeExists(employee.EmployeeId))
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
                    ModelState.AddModelError("Password", "Invalid Password\nPassword value must meet the following requirements: " +
                        "\n- Be at least 8 characters long" +
                        "\n- Contain no empty spaces" +
                        "\n- Contain at least one capital letter" +
                        "\n- Contain at least one number");
                }
            }
            return View(employee);
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(string id)
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

                var employee = await _context.Employee
                    .FirstOrDefaultAsync(m => m.EmployeeId == id);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks if the chosen employee iexists in the current context
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool EmployeeExists(string id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
        //---------------------------------------------------------------------------------------------------------------------------------
    }
}
//--------------------------------------------------End of Code------------------------------------------------------------