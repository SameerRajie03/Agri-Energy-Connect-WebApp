using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect_WebApp.Models
{
    public class Farmer
    {
        [Key]
        public int FarmerId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }

        public string? Username { get; set; }
        public string? Password { get; set; }

    }
}
