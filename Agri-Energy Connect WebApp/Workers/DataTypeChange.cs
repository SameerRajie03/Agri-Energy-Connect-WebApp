using System.Security.Cryptography;
using System.Text;

namespace Agri_Energy_Connect_WebApp.Workers
{
    public class DataTypeChange
    {
        //Method to change the password value to a fixed hashed value, for bettwer security
        public static string ToHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
        //----------------------------------------------------------------------------------------------------------------
    }
}
