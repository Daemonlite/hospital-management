using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health.Entities
{
    public class Shift
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public required User User { get; set; }  // Navigation to Doctor (User with Role.Doctor)

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public bool IsBooked { get; set; } = false;

        // For recurring shifts
        public RecurrencePattern? Recurrence { get; set; } 

        // Additional properties
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }



    public enum RecurrencePattern
    {
        None,
        Daily,
        Weekly,
        BiWeekly,
        Monthly
    }
}