using Health.Entities;
using Health.Models;
using Health.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Health.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController(IDepartmentService departmentService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Department?>>> GetDepartments()
        {
            var departments = await departmentService.GetDepartments();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<DepartmentDto?>> GetDepartmentById(Guid id)
        {
            var department = await departmentService.GetDepartmentById(id);
            if (department == null)
            {
                return BadRequest("Department not found");
            }
            return Ok(department);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Department?>> CreateDepartment(DepartmentCreateDto department)
        {
            var departments = await departmentService.CreateDepartment(department);

            if (departments == null)
            {
                return BadRequest("Department already exists");
            }
            return Ok(departments);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Department?>> UpdateDepartment(Guid id, DepartmentCreateDto department)
        {
            var departments = await departmentService.UpdateDepartment(id, department);
            if (departments == null)
            {
                return BadRequest("Department not found");
            }
            return Ok(departments);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult?> DeleteDepartment(Guid id)

        {
            var departments = await departmentService.DeleteDepartment(id);
            if (departments == null)
            {
                return BadRequest("Department not found");
            }
            return Ok("Department deleted successfully");
        }


    }

}