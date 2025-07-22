using Health.Data;
using Health.Entities;
using Health.Models;
using Microsoft.EntityFrameworkCore;

namespace Health.services
{
    public class DepartmentService(AppDbContext context) : IDepartmentService
    {
        public async Task<List<Department>> GetDepartments()
        {
            return await context.Departments.ToListAsync();
        }

        public async Task<Department?> GetDepartmentById(Guid id)
        {
            return await context.Departments
                .FirstOrDefaultAsync(d => d.Id == id);

        }

        public async Task<Department?> CreateDepartment(Department department)
        {
            if (await context.Departments.AnyAsync(d => d.Name == department.Name))
            {
                return null;
            }
            context.Departments.Add(department);
            await context.SaveChangesAsync();
            return department;
        }

        public async Task<Department?> UpdateDepartment(Department department)
        {
            context.Departments.Update(department);
            await context.SaveChangesAsync();
            return department;
        }
        

        public async Task<Department?> DeleteDepartment(Guid id)
        {
            var department = await context.Departments.FindAsync(id);
            if (department is null)
            {
                return null;
            }
            context.Departments.Remove(department);
            await context.SaveChangesAsync();
            return department;
        }
    }
}