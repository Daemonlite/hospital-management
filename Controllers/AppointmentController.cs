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

    public class AppointmentController(IAppointmentService _appointment, AppDbContext context) : ControllerBase
    {

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<AppointmentDto>>> GetAllAppointmentsAsync()
        {
            var appointments = await _appointment.GetAppointments();
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AppointmentDto>> GetAppointmentByIdAsync(Guid id)
        {
            var appointment = await _appointment.GetAppointmentById(id);
            return appointment == null ? NotFound() : Ok(appointment);
        }

        [HttpGet("patient/{id}")]
        [Authorize]
        public async Task<ActionResult<List<AppointmentDto>>> GetAppointmentsByPatientIdAsync(Guid id)
        {
            var appointments = await _appointment.GetAppointmentsByPatientId(id);
            return Ok(appointments);
        }

        [HttpGet("doctor/{id}")]
        [Authorize]
        public async Task<ActionResult<List<AppointmentDto>>> GetAppointmentsByUserIdAsync(Guid id)
        {
            var appointments = await _appointment.GetAppointmentsByUserId(id);
            return Ok(appointments);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AppointmentDto>> CreateAppointmentAsync(CreateAppointmentDto appointment)
        {
            Console.WriteLine(appointment);
            if(await context.Patients.AnyAsync(p => p.Id == appointment.PatientId) == false)
            {
                return BadRequest(new { error = "Patient not found" });
            }
            if (await context.Users.AnyAsync(u => u.Id == appointment.DoctorId) == false)
            {
                return BadRequest(new { error = "Doctor not found" });
            }
            if (await context.Shifts.AnyAsync(s => s.Id == appointment.AppointmentDateId) == false)
            {
                return BadRequest(new { error = "Shift not found" });
            }


            var shift = await context.Shifts.FindAsync(appointment.AppointmentDateId);

            if (shift == null)
            {
                return BadRequest(new { error = "Shift not found" });
            }

            if (shift.IsBooked) return BadRequest(new { error = "Shift is already booked" });

            if (shift.UserId != appointment.DoctorId && shift.IsBooked == true)
            {
                return BadRequest(new { error = "Shift is already booked" });
            }


            var newAppointment = await _appointment.CreateAppointment(appointment);

            return Ok(newAppointment);
        }

        [HttpPut("{id}")] // Update appointment by ID
        [Authorize]
        public async Task<ActionResult<AppointmentDto>> UpdateAppointmentAsync(Guid id, CreateAppointmentDto appointment)
        {
            var updatedAppointment = await _appointment.UpdateAppointment(id, appointment);
            return updatedAppointment == null ? NotFound(new { error = "Appointment not found" }) : Ok(updatedAppointment);
        }

        [HttpDelete("{id}")] // Delete appointment by ID
        [Authorize]
        public async Task<ActionResult<bool>> DeleteAppointmentAsync(Guid id)
        {
            var deleted = await _appointment.DeleteAppointment(id);
            return deleted ? Ok() : NotFound(new { error = "Appointment not found" });
        }
        
    }

} 