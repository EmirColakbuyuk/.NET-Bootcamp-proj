using System.ComponentModel.DataAnnotations;

namespace TechMarketApi.Models
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
        public string Category { get; set; } = "Smartwatch";  // Predefined category
    }
}
