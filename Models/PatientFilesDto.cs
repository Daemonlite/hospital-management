using System.ComponentModel.DataAnnotations;
using Health.Entities;

namespace Health.Models
{
    public class PatientFilesDto
    {
        [Required]
        public Guid PatientId { get; set; }

        public string Description { get; set; } = string.Empty;

        [Required]
        public IFormFile File { get; set; } = default!;
    }


    public class PatientFilesListDto
    {
        public Guid Id { get; set; }

        public Patients? Patient { get; set; }

        public string Description { get; set; } = string.Empty;


        public string FilePath { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string ContentType { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; }
    }

    public class SinglePatientFileDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public DateTime UploadedAt { get; set; }
    }

    public class PatientsDtos
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateOnly DOB { get; set; }

        public Patients? Patient { get; set; }

        public string Gender { get; set; } = string.Empty;

    }
}

