using System.ComponentModel.DataAnnotations.Schema;

namespace Health.Entities
{
    public class PatientsFiles
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; } 

        [ForeignKey("PatientId")]                         
        public Patients? Patient { get; set; }

        public string Description { get; set; } = string.Empty;           
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; } 

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
