
using Health.Models;

namespace Health.Entities
{
    public class Patients
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        public DateOnly DOB { get; set; }

        public string Gender { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;

        

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }

}