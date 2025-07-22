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
        [HttpGet("fetch-departments")]
        [Authorize]
        public async Task<ActionResult<List<Department>>> GetDepartments()
        {
            var departments = await departmentService.GetDepartments();
            return Ok(departments);
        }

        [HttpGet("fetch-department/{id}")]
        [Authorize]
        public async Task<ActionResult<Department?>> GetDepartmentById(Guid id)
        {
            var department = await departmentService.GetDepartmentById(id);
            if (department == null)
            {
                return BadRequest("Department not found");
            }
            return Ok(department);
        }

        [HttpPost("create-department")]
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

        [HttpPut("update-department/{id}")]
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

        [HttpDelete("delete-department/{id}")]
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