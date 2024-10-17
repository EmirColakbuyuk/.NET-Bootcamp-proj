using System.ComponentModel.DataAnnotations;

namespace TechMarketApi.Models
{
    public class Computer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Processor { get; set; } = string.Empty;

        [Required]
        public int RAM { get; set; }

        [Required]
        public int Storage { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Category { get; set; } = "Computer";  // Predefined category
    }
}
