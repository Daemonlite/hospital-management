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
            var department = await context.Departments.FindAsync(id);
            if (department is null)
            {
                return null;
            }
            return department;
        }

        public async Task<Department?> CreateDepartment(DepartmentCreateDto departmentDto)
        {
            if (await context.Departments.AnyAsync(d => d.Name == departmentDto.Name))
            {
                return null; // Department with this name already exists
            }

            var department = new Department
            {
                Name = departmentDto.Name,
                Description = departmentDto.Description
               
            };

            await context.Departments.AddAsync(department);
            await context.SaveChangesAsync();
            return department;
        }

        public async Task<Department?> UpdateDepartment(Guid id, DepartmentCreateDto departmentDto)
        {
            var existingDepartment = await context.Departments.FindAsync(id);
            if (existingDepartment is null)
            {
                return null; // Department not found
            }

            existingDepartment.Name = departmentDto.Name;
            existingDepartment.Description = departmentDto.Description;


            context.Departments.Update(existingDepartment);
            await context.SaveChangesAsync();
            return existingDepartment;
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