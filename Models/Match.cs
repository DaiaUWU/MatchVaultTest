

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatchVault.Models
{
    public class Match
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Match Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [Display(Name = "Wrestler 1")]
        public string Wrestler1 { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [Display(Name = "Wrestler 2")]
        public string Wrestler2 { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Promotion { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Event { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Match Date")]
        public DateTime MatchDate { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [Display(Name = "Match Image")]
        public string? ImageUrl { get; set; }

        // Relationship with wrestling event
        [Display(Name = "Event")]
        public int? EventId { get; set; }

        [ForeignKey(nameof(EventId))]
        public Event? WrestlingEvent { get; set; }

        // Additional match information
        [StringLength(100)]
        [Display(Name = "Match Type")]
        public string? MatchType { get; set; }

        [StringLength(150)]
        public string? Winner { get; set; }

        [Display(Name = "Duration (minutes)")]
        public int? DurationMinutes { get; set; }

        [Range(0, 10)]
        public double? Rating { get; set; }
    }
}
