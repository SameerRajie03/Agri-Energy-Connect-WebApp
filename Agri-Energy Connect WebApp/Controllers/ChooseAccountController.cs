using Agri_Energy_Connect_WebApp.Models;
using Agri_Energy_Connect_WebApp.Workers;
using Microsoft.AspNetCore.Mvc;

namespace Agri_Energy_Connect_WebApp.Controllers
{   
    public class ChooseAccountController : Controller
    {
        /// <summary>
        /// variables that are declared globally so that the view can save their choices
        /// </summary>
        public string? AccountType { get; set; }
        public String? EmployeeId { get; set; }
        public int? FarmerId { get; set; }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// when any class/view calls for this controller, the app's context is parsed to a variable in the constructer.
        /// the value of that variable is them assigned to a read only variable that is globally assigned for the class.
        /// </summary>
        private readonly Agri_Energy_Connect_WebApp.Data.Agri_Energy_Connect_WebAppContext _context;

        public ChooseAccountController(Agri_Energy_Connect_WebApp.Data.Agri_Energy_Connect_WebAppContext context)
        {
            _context = context;
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Loads the desired view
        /// </summary>
        /// <returns></returns>
        public IActionResult ChooseAccountView()
        {
            return View();
        }
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks that whichever id is input, matches one that exists in the context of the account type that the user selected
        /// </summary>
        /// <param name="accountType"></param>
        /// <param name="employeeId"></param>
        /// <param name="farmerId"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
    }
}
//--------------------------------------------------End of Code------------------------------------------------------------