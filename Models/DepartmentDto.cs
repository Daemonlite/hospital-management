using System.ComponentModel.DataAnnotations;
using Health.Entities;

namespace Health.Models
{
    public class DepartmentCreateDto
    {
        
        [Required(ErrorMessage = "Department name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department description is required")]
        public string Description { get; set; } = string.Empty;
    }

    public class DepartmentListDto()
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User? Users { get; set; }
    }
}