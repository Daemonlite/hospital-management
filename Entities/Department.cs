namespace Health.Entities
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<User>? Users { get; set; }  // Navigation property

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}