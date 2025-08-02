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
    public class RecordsController(IRecordsService recordsService) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Records>>> GetRecordsAsync()
        {
            var records = await recordsService.GetAllRecords();
            return Ok(records);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<RecordsDto>> GetRecordsByIdAsync(Guid id)
        {
            var records = await recordsService.GetRecordsById(id);

            return records == null ? NotFound() : Ok(records);

        }

        [HttpGet("patient/{id}")]
        [Authorize]
        public async Task<ActionResult<List<RecordsDto>>> GetRecordsByPatientId(Guid id)
        {
            var records = await recordsService.GetRecordsByPatientId(id);
            return records == null ? NotFound() : Ok(records);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RecordsDto>> AddRecords(CreateRecordsDto records)
        {
            var recordsDto = await recordsService.AddRecords(records);

            if (recordsDto is null)
            {
                return NotFound(new { Error = "Patient or Doctor not found" });
            }
            else
            {
                return Ok(recordsDto);
            }
        }

        [HttpPut("{id}")] // Update a record by ID
        [Authorize]
        public async Task<ActionResult<RecordsDto>> UpdateRecords(Guid id, CreateRecordsDto records)
        {
            var updatedRecord = await recordsService.UpdateRecords(id, records);
            return updatedRecord == null ? NotFound(new { error = "Record not found" }) : Ok(updatedRecord);
        }
        
        [HttpDelete("{id}")] // Delete a record by ID
        [Authorize]
        public async Task<ActionResult<bool>> DeleteRecords(Guid id)
        {
            var deleted = await recordsService.DeleteRecords(id);
            return deleted ? Ok() : NotFound(new { error = "Record not found" });
        }
    };

    
}