using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect_WebApp.Models
{
    public class ChooseAccountModel
    {
        [Required]
        public string? AccountType { get; set; }
        [Required]
        public String? EmployeeId { get; set; }
        [Required]
        public int? FarmerId { get; set; }
    }
}
