using Agri_Energy_Connect_WebApp.Data;
using Agri_Energy_Connect_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Agri_Energy_Connect_WebApp.Workers
{
    public class Validation
    {
        public static bool NotEmpty(string input)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsInt(string input)
        {
            if (NotEmpty(input))
            {
                if (int.TryParse(input, out int value))
                {
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

        public static bool IsDouble(string input)
        {
            if (NotEmpty(input))
            {
                if (double.TryParse(input, out double value))
                {
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

        public static bool IsDate(string input)
        {
            if (NotEmpty(input))
            {
                if (DateTime.TryParse(input, out DateTime value))
                {
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

        public static bool FarmerExists(string input, Agri_Energy_Connect_WebAppContext context)
        {
            var farmers = context.Farmer.ToList();
            Farmer? farmer = farmers.Where(u => u.Username == input).FirstOrDefault();

            if (farmer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool FarmerUsername(Farmer? farmer, Agri_Energy_Connect_WebAppContext context)
        {

            if (farmer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool EmployeeExists(string input, Agri_Energy_Connect_WebAppContext context)
        {
            var employees = context.Employee.ToList();
            Employee? employee = employees.Where(u => u.Username == input).FirstOrDefault();

            if (employee == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool EmployeeUsername(Employee? employee, Agri_Energy_Connect_WebAppContext context)
        {
            var employees = context.Employee.ToList();
            int amount = employees.Where(u => u.Username == employee.Username).ToList().Count();
            if (amount < 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool EmployeeExistsId(string input, Agri_Energy_Connect_WebAppContext context)
        {
            var employees = context.Employee.ToList();
            Employee? employee = employees.Where(u => u.EmployeeId == input).FirstOrDefault();

            if (employee == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool FarmerExistsId(string input, Agri_Energy_Connect_WebAppContext context)
        {
            var farmers = context.Farmer.ToList();
            Farmer? farmer = farmers.Where(u => u.FarmerId.ToString() == input).FirstOrDefault();

            if (farmer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool UsernameExists(string input, Agri_Energy_Connect_WebAppContext context)
        {
            if (EmployeeExists(input, context) || FarmerExists(input, context))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ValidPassword(string input)
        {
            //Password can't be empty
            if (!string.IsNullOrEmpty(input))
            {
                //Password can't contain any empty spaces
                if (!input.Any(char.IsWhiteSpace))
                {
                    //Password Must be at least 8 characters long
                    if (input.Length >= 8)
                    {
                        //Password must contain at least one capital letter
                        if (input.Any(char.IsUpper))
                        {
                            //Password must contain at least one number
                            if (input.Any(char.IsNumber))
                            {
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
            else
            {
                return false;
            }
        }

        public static IActionResult UserLoggedIn(int farmer, string employee)
        {
            if (NotEmpty(employee) || farmer > 0)
            {
                return null;
            }
            else
            {
                return new RedirectToActionResult("Login", "LoginView", null);
            }
        }

    }
}
