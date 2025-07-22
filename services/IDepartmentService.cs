using Health.Entities;
using Health.Models;

namespace Health.services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetDepartments();
        Task<Department?> GetDepartmentById(Guid id);
        Task<Department?> CreateDepartment(DepartmentCreateDto department);
        Task<Department?> UpdateDepartment( Guid id,DepartmentCreateDto department);
        Task<Department?> DeleteDepartment(Guid id);
        // Task<User?> GetDepartmentWorkers(int id);
    }
}