using Health.Data;
using Health.Entities;
using Health.Models;
using Health.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Health.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PrescriptionsController(IPrescriptionService _prescription) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<PrescriptionListDto>>> GetAllPrescriptions()
        {
            var prescriptions = await _prescription.GetAllPrescriptions();
            return Ok(prescriptions);
        }

        [HttpGet("{id}")]
        [Authorize]

        public async Task<ActionResult<List<PrescriptionListDto>>> GetPrescriptionById(Guid id)
        {
            var prescriptions = await _prescription.GetPrescriptionById(id);
            return Ok(prescriptions);
        }

        [HttpGet("patient/{id}")] // Get prescriptions by Patient ID
        [Authorize]
        public async Task<ActionResult<List<PrescriptionListDto>>> GetPrescriptionByPatientId(Guid id)
        {
            var prescriptions = await _prescription.GetPrescriptionByPatientId(id);
            return Ok(prescriptions);
        }

        [HttpGet("doctor/{id}")] // Get prescriptions by Doctor ID
        [Authorize]
        public async Task<ActionResult<List<PrescriptionListDto>>> GetPrescriptionByDoctorId(Guid id)
        {
            var prescriptions = await _prescription.GetPrescriptionByDoctorId(id);
            return Ok(prescriptions);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PrescriptionsCreateDto>> CreatePrescription(PrescriptionsCreateDto prescriptionsDto)
        {
            var result = await _prescription.CreatePrescription(prescriptionsDto);

            if (result == null)
            {
                return BadRequest(new {success = false, error = "Patient or doctor not found." });
            }
            return Ok(result);
        }


        [HttpPut("{id}")] // Update a prescription
        [Authorize]
        public async Task<ActionResult<PrescriptionsCreateDto>> UpdatePrescription(Guid id, PrescriptionsCreateDto prescriptionsDto)
        {
            var result = await _prescription.UpdatePrescription(id, prescriptionsDto);

            if (result == null)
            {
                return BadRequest(new {success=false, error = "Patient or doctor not found." });
            }
            return Ok(result);
        }

        [HttpDelete("{id}")] // Delete a prescription
        [Authorize]
        public async Task<ActionResult<bool>> DeletePrescription(Guid id)
        {
            var result = await _prescription.DeletePrescription(id);
            if (!result)
            {
                return BadRequest(new {success=false, error = "Prescription not found." });
            }
            return Ok(new {success=true, message = "Prescription deleted successfully." });
        }
    }
}