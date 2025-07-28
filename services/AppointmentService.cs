using Health.Data;
using Health.Entities;
using Health.Models;
using Microsoft.EntityFrameworkCore;

namespace Health.services
{
    public class AppointmentService(AppDbContext context) : IAppointmentService
    {
        public async Task<List<AppointmentDto>> GetAppointments()
        {
            return await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.AppointmentDate)
                .OrderBy(a => a.AppointmentDate)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    Patient = a.Patient == null ? null : new PatientsDto { Id = a.Patient.Id, FullName = a.Patient.FullName },
                    Doctor = a.Doctor == null ? null : new UserDto { Id = a.Doctor.Id, FullName = a.Doctor.FullName, Email = a.Doctor.Email },
                    AppointmentDate = a.AppointmentDate == null ? null : new ShiftResponseDto 
                    { 
                        Id = a.AppointmentDate.Id,
                        StartTime = a.AppointmentDate.StartTime, 
                        EndTime = a.AppointmentDate.EndTime 
                    },
                })
                .ToListAsync();
        }

        public async Task<AppointmentDto?> GetAppointmentById(Guid id)
        {
            return await context.Appointments
                .Where(a => a.Id == id)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    Patient = a.Patient == null ? null : new PatientsDto { Id = a.Patient.Id, FullName = a.Patient.FullName },
                    Doctor = a.Doctor == null ? null : new UserDto { Id = a.Doctor.Id, FullName = a.Doctor.FullName, Email = a.Doctor.Email },
                    AppointmentDate = a.AppointmentDate == null ? null : new ShiftResponseDto 
                    { 
                        Id = a.AppointmentDate.Id,
                        StartTime = a.AppointmentDate.StartTime, 
                        EndTime = a.AppointmentDate.EndTime 
                    },
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPatientId(Guid id)
        {
            return await context.Appointments
                .Where(a => a.PatientId == id)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    Patient = a.Patient == null ? null : new PatientsDto { Id = a.Patient.Id, FullName = a.Patient.FullName },
                    Doctor = a.Doctor == null ? null : new UserDto { Id = a.Doctor.Id, FullName = a.Doctor.FullName, Email = a.Doctor.Email },
                    AppointmentDate = a.AppointmentDate == null ? null : new ShiftResponseDto 
                    { 
                        Id = a.AppointmentDate.Id,
                        StartTime = a.AppointmentDate.StartTime, 
                        EndTime = a.AppointmentDate.EndTime 
                    },
                })
                .ToListAsync();
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByUserId(Guid id)
        {
            return await context.Appointments
                .Where(a => a.DoctorId == id)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    Patient = a.Patient == null ? null : new PatientsDto { Id = a.Patient.Id, FullName = a.Patient.FullName },
                    Doctor = a.Doctor == null ? null : new UserDto { Id = a.Doctor.Id, FullName = a.Doctor.FullName, Email = a.Doctor.Email },
                    AppointmentDate = a.AppointmentDate == null ? null : new ShiftResponseDto 
                    { 
                        Id = a.AppointmentDate.Id,
                        StartTime = a.AppointmentDate.StartTime, 
                        EndTime = a.AppointmentDate.EndTime 
                    },
                })
                .ToListAsync();
        }

        public async Task<CreateAppointmentDto> CreateAppointment(CreateAppointmentDto appointmentDto)
        {
            var shift = await context.Shifts.FindAsync(appointmentDto.AppointmentDateId) ?? throw new ArgumentException("Shift not found");
            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                AppointmentDate = await context.Shifts.FindAsync(appointmentDto.AppointmentDateId),
            };

            context.Appointments.Add(appointment);
            shift.IsBooked = true;
            await context.SaveChangesAsync();

            return appointmentDto;
        }

        public async Task<CreateAppointmentDto?> UpdateAppointment(Guid id, CreateAppointmentDto appointmentDto)
        {
            var existingAppointment = await context.Appointments.FindAsync(id);
            if (existingAppointment == null)
            {
                return null;
            }

            var patient = await context.Patients.FindAsync(appointmentDto.PatientId);
            var doctor = await context.Users.FindAsync(appointmentDto.DoctorId);
            var shift = await context.Shifts.FindAsync(appointmentDto.AppointmentDateId);

            if (patient == null || doctor == null || shift == null)
            {
                throw new ArgumentException("Patient, Doctor, or Shift not found");
            }

            existingAppointment.PatientId = appointmentDto.PatientId;
            existingAppointment.DoctorId = appointmentDto.DoctorId;
            existingAppointment.AppointmentDate = shift;

            await context.SaveChangesAsync();
            return appointmentDto;
        }

        public async Task<bool> DeleteAppointment(Guid id)
        {
            var appointment = await context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return false;
            }

            context.Appointments.Remove(appointment);
            await context.SaveChangesAsync();
            return true;
        }
    }
}