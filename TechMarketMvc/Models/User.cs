using System.ComponentModel.DataAnnotations;

namespace TechMarketMvc.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
