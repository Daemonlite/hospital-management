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
    public class PatientFilesController(IPatientFilesService patientFilesService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<PatientsFiles>>> GetPatientFiles()
        {
            var patientFiles = await patientFilesService.FetchAllFiles();
            return Ok(patientFiles);
        }

        [HttpGet("patient/{id}")]
        [Authorize]
        public async Task<ActionResult<PatientsFiles>> GetPatientFileById(Guid id)
        {
            var patientFile = await patientFilesService.GetPatientFilesByPatientId(id);

            if (patientFile is null)
            {
                return NotFound("Patient file not found");
            }
            return Ok(patientFile);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PatientsFiles>> AddPatientFile(PatientFilesDto patientFile)
        {
            var patientFileAdded = await patientFilesService.AddPatientFile(patientFile);
            return Ok(patientFileAdded);
        }

        [HttpDelete("{file_id}")] 
        [Authorize]
        public async Task<IActionResult> DeletePatientFile(Guid file_id)
        {
            var deleted = await patientFilesService.DeletePatientFile(file_id);
            return deleted ? Ok("Patient file deleted successfully") : BadRequest("Patient file not found");
        }
    }
}