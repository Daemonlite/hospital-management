using System.ComponentModel.DataAnnotations;
namespace Health.Models
{
    public class PatientsDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public DateOnly DOB { get; set; }

        public string Gender { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PatientsCreateDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateOnly DOB { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;
        [Required]
        public string ContactInfo { get; set; } = string.Empty;
    }


    public class FinalUploadDto
    {
        public string AbsoluteUrl { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public int FileSize { get; set; }

        public string FileName { get; set; } = string.Empty;
    }


}