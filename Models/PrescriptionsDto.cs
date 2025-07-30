using System.ComponentModel.DataAnnotations;

namespace Health.Models
{
    public class PrescriptionsCreateDto
    {
        [Required(ErrorMessage = "Prescription is required")]
        public string? Prescription { get; set; }

        [Required(ErrorMessage = "Patient is required")]
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "Doctor is required")]
        public Guid DoctorId { get; set; }

    }

    public class PrescriptionListDto
    {
        public Guid Id { get; set; }
        public string? Prescription { get; set; }

        public PatientsDto? Patient { get; set; }

        public UserDto? Doctor { get; set; }

        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}

