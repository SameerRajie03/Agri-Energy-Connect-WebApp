using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect_WebApp.Models
{
    public class Employee
    {
        [Key]
        public string EmployeeId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
