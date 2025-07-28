

using System.ComponentModel.DataAnnotations;
using Health.Entities;

namespace Health.Models
{
    public class CreateAppointmentDto
    {
        [Required(ErrorMessage = "Patient ID is required")]
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "Doctor ID is required")]
        public Guid DoctorId { get; set; }

        [Required(ErrorMessage = "Shift ID is required")]
        public Guid? AppointmentDateId { get; set; }
    }

    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public PatientsDto? Patient { get; set; }
        public UserDto? Doctor { get; set; }
        public ShiftResponseDto? AppointmentDate { get; set; }

        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}