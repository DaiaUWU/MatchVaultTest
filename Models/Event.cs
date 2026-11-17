
using System;
using System.ComponentModel.DataAnnotations;

namespace MatchVault.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Promotion { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }

        [StringLength(150)]
        public string? Venue { get; set; }

        [StringLength(150)]
        public string? Location { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [Display(Name = "Event Poster")]
        public string? ImageUrl { get; set; }
    }
}
