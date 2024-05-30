using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect_WebApp.Data;
using Agri_Energy_Connect_WebApp.Models;

namespace Agri_Energy_Connect_WebApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly Agri_Energy_Connect_WebAppContext _context;

        public EmployeesController(Agri_Energy_Connect_WebAppContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                return loginCheckResult;
            }
            else
            {
                return View(await _context.Employee.ToListAsync());
            }
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
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

        // GET: Employees/Create
        public IActionResult Create()
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

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,Name,Surname,Username,Password")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Username = employee.EmployeeId.ToString();
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
                return loginCheckResult;
            }
            else
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
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
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
                return RedirectToRoute("/Login/Login");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var loginCheckResult = Workers.Validation.UserLoggedIn(Workers.GetSet.UserFarmer, Workers.GetSet.UserEmployee);

            if (loginCheckResult != null)
            {
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

        private bool EmployeeExists(string id)
        {
            return _context.Employee.Any(e => e.EmployeeId == id);
        }
    }
}
