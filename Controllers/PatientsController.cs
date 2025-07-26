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
    public class PatientsController(IPatientsService patientsService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Patients?>>> GetPatients()
        {
            var patients = await patientsService.GetAllPatients();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Patients?>> GetPatientById(Guid id)
        {
            var patient = await patientsService.GetPatientById(id);
            return patient == null ? NotFound() : Ok(patient);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Patients?>> AddPatient(PatientsCreateDto request)
        {
            var addedPatient = await patientsService.AddPatient(request);
            if (addedPatient == null)
            {
                return BadRequest("Patient already exists");
            }
            return Ok(addedPatient);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Patients?>> UpdatePatient(Guid id,PatientsCreateDto patient)
        {
            var updatedPatient = await patientsService.UpdatePatient(id,patient);
            return updatedPatient == null ? BadRequest("Patient not found") : Ok(updatedPatient);
        }

        [HttpDelete("{id}")] 
        [Authorize]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var deleted = await patientsService.DeletePatient(id);
            return deleted ? Ok("Patient deleted successfully") : BadRequest("Patient not found");
        }
    }
}