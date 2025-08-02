

using System.ComponentModel.DataAnnotations.Schema;

namespace Health.Entities
{
    public class Records
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }

        public Patients? Patient { get; set; }
        public User? Doctor { get; set; }

        public string? PatientsFileIds { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
    }
}