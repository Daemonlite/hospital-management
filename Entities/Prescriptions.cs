

using System.ComponentModel.DataAnnotations.Schema;

namespace Health.Entities
{
    public class Prescriptions
    {
        public Guid Id { get; set; }
        public string? Prescription { get; set; }
        public Guid PatientId { get; set; }
        [ForeignKey("PatientId")]
        public Patients? Patient { get; set; }

        public Guid DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public User? Doctor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}