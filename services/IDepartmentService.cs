using Health.Entities;
using Health.Models;

namespace Health.services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetDepartments();
        Task<Department?> GetDepartmentById(Guid id);
        Task<Department?> CreateDepartment(Department department);
        Task<Department?> UpdateDepartment(Department department);
        Task<Department?> DeleteDepartment(Guid id);
        // Task<User?> GetDepartmentWorkers(int id);
    }
}