using System.ComponentModel.DataAnnotations;
using Health.Entities;

namespace Health.Models
{
    public class CreateRecordsDto
    {
        [Required(ErrorMessage = "PatientId is required")]
        public Guid PatientId { get; set; }

        [Required(ErrorMessage = "DoctorId is required")]
        public Guid DoctorId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        public string? Diagnosis { get; set; }

        [Required(ErrorMessage = "Treatment")]
        public string? Treatment { get; set; }
    }

    // public class UpdateRecordsDto : CreateRecordsDto
    // {
    //     public Guid Id { get; set; }
    // }


    public class RecordsDto
    {
        public Guid Id { get; set; }
        public PatientsDto? Patient { get; set; }
        public UserDto? Doctor { get; set; }
        public DateTime Date { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}