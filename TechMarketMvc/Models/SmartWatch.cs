using System.ComponentModel.DataAnnotations;

namespace TechMarketMvc.Models
{
    public class Smartwatch
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public int BatteryLife { get; set; }

        [Required]
        public bool HasGPS { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; } = "Smartwatch";  

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;  

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number")]
        public int Stock { get; set; }  

        [Required]
        public string ImagePath { get; set; } = string.Empty;  // Path to the static image
    }
}
