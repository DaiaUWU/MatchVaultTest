
using System.ComponentModel.DataAnnotations;

namespace MatchVault.Models
{
    public class Wrestler
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Ring Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(150)]
        public string? Promotion { get; set; }

        [StringLength(2000)]
        public string? Biography { get; set; }

        [Display(Name = "Profile Image")]
        public string? ImageUrl { get; set; }

        public DateTime? DebutDate { get; set; }
    }
}
