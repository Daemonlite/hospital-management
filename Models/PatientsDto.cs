namespace Health.Models
{
    public class PatientsDto
    {
        public string FullName { get; set; } = string.Empty;

        public DateOnly DOB { get; set; }

        public string Gender { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
    }
}