using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect_WebApp.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int FarmerId { get; set; }

        public Category? Category { get; set; }
        public Farmer? Farmer { get; set; }
    }
}
