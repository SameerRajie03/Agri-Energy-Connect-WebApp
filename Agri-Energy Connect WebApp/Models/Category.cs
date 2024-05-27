using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect_WebApp.Models
{
    public class Category
    {
        [Key]
        public int CategporyId { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}
