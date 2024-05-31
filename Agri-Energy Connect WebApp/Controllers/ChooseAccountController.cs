using Agri_Energy_Connect_WebApp.Models;
using Agri_Energy_Connect_WebApp.Workers;
using Microsoft.AspNetCore.Mvc;

namespace Agri_Energy_Connect_WebApp.Controllers
{   
    public class ChooseAccountController : Controller
    {
        private readonly Agri_Energy_Connect_WebApp.Data.Agri_Energy_Connect_WebAppContext _context;
        public string? AccountType { get; set; }
        public String? EmployeeId { get; set; }
        public int? FarmerId { get; set; }

        public ChooseAccountController(Agri_Energy_Connect_WebApp.Data.Agri_Energy_Connect_WebAppContext context)
        {
            _context = context;
        }

        public IActionResult ChooseAccountView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessAccountType(string accountType, string employeeId, string farmerId)
        {
            if (accountType == "Employee")
            {
                if (!Validation.EmployeeExistsId(employeeId, _context))
                {
                    ModelState.AddModelError("EmployeeId", "Must Enter an Existing Employee ID");
                    TempData["ErrorMessage"] = "Must enter an Existing Employee ID";
                    return View("ChooseAccountView");
                }
                else
                {
                    return RedirectToAction("Edit", "Employees", new { id = employeeId });
                }
            }
            else if (accountType == "Farmer")
            {
                if (!Validation.FarmerExistsId(farmerId, _context))
                {
                    ModelState.AddModelError("FarmerId", "Must Enter an Existing Farmer ID");
                    TempData["ErrorMessage"] = "Must enter a valid Employee ID";
                    return View("ChooseAccountView");
                }
                else
                {
                    return RedirectToAction("Edit", "Farmers", new { id = farmerId });
                }
            }
            else
            {
                return View("ChooseAccountView");
            }
        }
    }
}
