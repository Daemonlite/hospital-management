using Health.Data;
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
    public class ShiftController(IShiftService _shiftService, AppDbContext _context) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<ShiftDto>>> GetAllShiftsAsync()
        {
            var shifts = await _shiftService.GetAllShifts();
            return Ok(shifts);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ShiftDto>> GetShiftByIdAsync(Guid id)
        {
            var shift = await _shiftService.GetShiftById(id);
            return shift == null ? NotFound(new { error = "Shift not found" }) : Ok(shift);
        }

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult<List<ShiftDto>>> GetAvailableSlotsByUserIdAsync(Guid userId, DateTime date)
        {
            var slots = await _shiftService.GetAvailableSlotsByUserId(userId, date);
            return slots == null ? NotFound(new { error = "Slots not found" }) : Ok(slots);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ShiftDto>> CreateShiftAsync(CreateShiftDto shiftDto)
        {
            if (await _context.Users.FindAsync(shiftDto.UserId) == null)
            {
                 return BadRequest(new { error = "User does not exist" });
            }
            // Validate time range
            if (shiftDto.EndTime <= shiftDto.StartTime)
            {
                return BadRequest(new { error = "End time must be after start time" });
            }

            var shift = await _shiftService.CreateShift(shiftDto);
            return Ok(shift);
        }


        [HttpPut("{id}")] // Update shift by ID
        [Authorize]
        public async Task<ActionResult<ShiftDto>> UpdateShiftAsync(Guid id, CreateShiftDto shiftDto)
        {
            var updatedShift = await _shiftService.UpdateShift(id, shiftDto);
            return updatedShift == null ? NotFound(new { error = "Shift not found" }) : Ok(updatedShift);
        }


        [HttpDelete("{id}")] // Delete shift by ID
        [Authorize]
        public async Task<ActionResult<bool>> DeleteShiftAsync(Guid id)
        {
            var deleted = await _shiftService.DeleteShift(id);
            return deleted ? Ok() : NotFound(new { error = "Shift not found" });
        }


    }
    

}