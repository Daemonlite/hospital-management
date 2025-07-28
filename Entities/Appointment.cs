

using System.ComponentModel.DataAnnotations.Schema;

namespace Health.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patients? Patient { get; set; }
        public Guid ShiftId { get; set; }

        [ForeignKey("ShiftId")]
        public Shift? AppointmentDate { get; set; }

        public Guid DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public User? Doctor { get; set; }

        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}