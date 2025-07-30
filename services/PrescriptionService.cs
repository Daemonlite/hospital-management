using Health.Data;
using Health.Models;
using Health.Entities;
using Microsoft.EntityFrameworkCore;

namespace Health.services
{
    public class PrescriptionService(AppDbContext context, EmailService mail) : IPrescriptionService
    {
        public async Task<List<PrescriptionListDto>> GetAllPrescriptions()
        {
            return await context.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Doctor)
            .OrderBy(p => p.CreatedAt)
            .Select(p => new PrescriptionListDto
            {
                Id = p.Id,
                Prescription = p.Prescription,
                Patient = p.Patient == null ? null : new PatientsDto { Id = p.Patient.Id, FullName = p.Patient.FullName },
                Doctor = p.Doctor == null ? null : new UserDto { Id = p.Doctor.Id, FullName = p.Doctor.FullName, Email = p.Doctor.Email }
            }).ToListAsync();
        }

        public async Task<List<PrescriptionListDto>> GetPrescriptionByDoctorId(Guid id)
        {
            return await context.Prescriptions
            .Where(p => p.DoctorId == id)
            .OrderBy(p => p.CreatedAt)
            .Select(p => new PrescriptionListDto
            {
                Id = p.Id,
                Prescription = p.Prescription,
                Patient = p.Patient == null ? null : new PatientsDto { Id = p.Patient.Id, FullName = p.Patient.FullName },
                Doctor = p.Doctor == null ? null : new UserDto { Id = p.Doctor.Id, FullName = p.Doctor.FullName, Email = p.Doctor.Email }
            }).ToListAsync();
        }

        public async Task<List<PrescriptionListDto>> GetPrescriptionById(Guid id)
        {
            return await context.Prescriptions
            .Where(p => p.Id == id)
            .Select(p => new PrescriptionListDto
            {
                Id = p.Id,
                Prescription = p.Prescription,
                Patient = p.Patient == null ? null : new PatientsDto { Id = p.Patient.Id, FullName = p.Patient.FullName },
                Doctor = p.Doctor == null ? null : new UserDto { Id = p.Doctor.Id, FullName = p.Doctor.FullName, Email = p.Doctor.Email }
            }).ToListAsync();
        }

        public async Task<List<PrescriptionListDto>> GetPrescriptionByPatientId(Guid id)
        {
            return await context.Prescriptions
             .Where(p => p.PatientId == id)
             .OrderBy(p => p.CreatedAt)
             .Select(p => new PrescriptionListDto
             {
                 Id = p.Id,
                 Prescription = p.Prescription,
                 Patient = p.Patient == null ? null : new PatientsDto { Id = p.Patient.Id, FullName = p.Patient.FullName },
                 Doctor = p.Doctor == null ? null : new UserDto { Id = p.Doctor.Id, FullName = p.Doctor.FullName, Email = p.Doctor.Email }
             }).ToListAsync();
        }
        public async Task<PrescriptionsCreateDto?> CreatePrescription(PrescriptionsCreateDto prescriptionsDto)
        {
            var patient = await context.Patients.FindAsync(prescriptionsDto.PatientId);
            var doctor = await context.Users.FindAsync(prescriptionsDto.DoctorId);
            if (patient == null || doctor == null)
            {
                return null;
            }

            var newPrescription = new Prescriptions
            {
                Prescription = prescriptionsDto.Prescription,
                PatientId = prescriptionsDto.PatientId,
                DoctorId = prescriptionsDto.DoctorId,
                Doctor = doctor,
                Patient = patient
            };
            await context.Prescriptions.AddAsync(newPrescription);
            await context.SaveChangesAsync();
            // Send email
            if(patient.ContactInfo != null) await mail.SendEmailAsync(patient.ContactInfo, "Medical Prescription", $"Your medical prescription from {doctor.FullName} is: {prescriptionsDto.Prescription}");
            return prescriptionsDto;
        }

        public async Task<PrescriptionsCreateDto?> UpdatePrescription(Guid id, PrescriptionsCreateDto prescriptionsDto)
        {
            var prescription = await context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                return null;
            }

            var patient = await context.Patients.FindAsync(prescriptionsDto.PatientId);
            var doctor = await context.Users.FindAsync(prescriptionsDto.DoctorId);
            if (patient == null || doctor == null)
            {
                return null;
            }
            prescription.Prescription = prescriptionsDto.Prescription;
            prescription.PatientId = prescriptionsDto.PatientId;
            prescription.DoctorId = prescriptionsDto.DoctorId;
            prescription.Doctor = doctor;
            prescription.Patient = patient;
            await context.SaveChangesAsync();
            return prescriptionsDto;
        }

        public async Task<bool> DeletePrescription(Guid id)
        {
            var prescription = await context.Prescriptions.FindAsync(id);
            if (prescription == null)
            {
                return false;
            }
            context.Prescriptions.Remove(prescription);
            await context.SaveChangesAsync();
            return true;
        }
    }
}