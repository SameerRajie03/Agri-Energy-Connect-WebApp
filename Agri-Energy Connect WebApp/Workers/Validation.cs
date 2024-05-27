using Agri_Energy_Connect_WebApp.Data;
using Agri_Energy_Connect_WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Agri_Energy_Connect_WebApp.Workers
{
    public class Validation
    {
        public bool NotEmpty(string input)
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

        public bool IsInt(string input)
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

        public bool IsDouble(string input)
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

        public bool IsDate(string input)
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

        public bool FarmerExists(string input, Agri_Energy_Connect_WebAppContext context)
        {
            Farmer? farmer = context.Farmer.Where(u => u.Username == input).FirstOrDefault();

            if (farmer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool EmployeeExists(string input, Agri_Energy_Connect_WebAppContext context)
        {
            Employee? employee = context.Employee.Where(u => u.Username == input).FirstOrDefault();

            if (employee == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool UsernameExists(string input, Agri_Energy_Connect_WebAppContext context)
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

        public bool ValidPassword(string input)
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

    }
}
