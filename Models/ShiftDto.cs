using System.ComponentModel.DataAnnotations;

using Health.Entities;
namespace Health.Models
{
    // For creating new shifts
    public class CreateShiftDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Notes is required")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "Recurrence is required")]
        public string? Recurrence { get; set; }
    }

    // For displaying shifts
    public class ShiftDto
    {
        public Guid Id { get; set; }
        public UserDto? User { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string? Recurrence { get; set; }
        public bool IsAvailable => !IsBooked;
        public bool IsBooked { get; set; }
        public string? Notes { get; set; }
    }

    public class ShiftResponseDto
    {
        public Guid? Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}