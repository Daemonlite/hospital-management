using Health.Models;

namespace Health.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public Role Role { get; set; }

        // Foreign key relationship
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; } // Navigation property

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiryTime { get; set; }

        public List<PatientsFiles> Files { get; set; } = [];

        public List<Appointment> Appointments { get; set; } = []; // Navigation property>

    }
}
