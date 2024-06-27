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
        /// <summary>
        /// Checks that the input value has a value
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks that the input value is an Integer
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks that the input value is a Double
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks that the input value is a Date
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Chects if the parsed username matches a farmer in the parsed app context
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks if the Farmer exists in the parsed app context
        /// </summary>
        /// <param name="farmer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks if the parsed username matches an employee in the parsed app context
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks if the parsed Employee matches an Employee in the parsed app context
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks if the parsed id matches that of an Employee in the current app Context
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks that the parsed input matches a FarmerID that exists in the parsed context
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks if the input matches any username that exists in the parsed context
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// checks that the input value meets all the requirements for a password
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks that a user is logged into the application.
        /// 
        /// If net redirects them to the login page
        /// </summary>
        /// <param name="farmer"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
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
        //---------------------------------------------------------------------------------------------------------------------------------
    }
}
//--------------------------------------------------End of Code------------------------------------------------------------