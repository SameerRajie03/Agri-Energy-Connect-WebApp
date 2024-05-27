using Agri_Energy_Connect_WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agri_Energy_Connect_WebApp.Controllers
{
    public class ChooseAccountController : Controller
    {
        public IActionResult ChooseAccountView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessAccountType(string accountType, string employeeId, string farmerId)
        {
            if (accountType == "Employee")
            {
                // Assuming you have an action named EmployeeLogin in EmployeeController
                return RedirectToAction("Edit", "Employees", new { id = employeeId });
            }
            else if (accountType == "Farmer")
            {
                // Assuming you have an action named FarmerLogin in FarmerController
                return RedirectToAction("Edit", "Farmers", new { id = farmerId });
            }

            // In case of invalid input, stay on the same page
            return View("ChooseAccountView");
        }
    }
}
