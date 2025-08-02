using Health.Data;
using Health.Models;
using Health.Entities;
using Microsoft.EntityFrameworkCore;

namespace Health.services
{
    public class RecordsService(AppDbContext context) : IRecordsService
    {
        public async Task<List<RecordsDto>>GetAllRecords()
        {
            return await context.Records
            .Include(r => r.Patient)
            .Include(r => r.Doctor)
            .Select(r => new RecordsDto
            {
                Id = r.Id,
                Diagnosis = r.Diagnosis,
                Treatment = r.Treatment,
                Patient = r.Patient == null ? null : new PatientsDto { Id = r.Patient.Id, FullName = r.Patient.FullName },
                Doctor = r.Doctor == null ? null : new UserDto { Id = r.Doctor.Id, FullName = r.Doctor.FullName, Email = r.Doctor.Email },
            }).ToListAsync();
        }
        public async Task<List<RecordsDto>> GetRecordsByPatientId(Guid id)
        {
            return await context.Records
            .Where(r => r.PatientId == id)
            .Select(r => new RecordsDto
            {
                Id = r.Id,
                Diagnosis = r.Diagnosis,
                Treatment = r.Treatment,
                Patient = r.Patient == null ? null : new PatientsDto { Id = r.Patient.Id, FullName = r.Patient.FullName },
                Doctor = r.Doctor == null ? null : new UserDto { Id = r.Doctor.Id, FullName = r.Doctor.FullName, Email = r.Doctor.Email },
            }).ToListAsync();
        }


        public async Task<RecordsDto?>GetRecordsById(Guid id)
        {
            return await context.Records
            .Where(r => r.Id == id)
            .Select(r => new RecordsDto
            {
                Id = r.Id,
                Diagnosis = r.Diagnosis,
                Treatment = r.Treatment,
                Patient = r.Patient == null ? null : new PatientsDto { Id = r.Patient.Id, FullName = r.Patient.FullName },
                Doctor = r.Doctor == null ? null : new UserDto { Id = r.Doctor.Id, FullName = r.Doctor.FullName, Email = r.Doctor.Email },
            }).FirstOrDefaultAsync();
        }

        public async Task<RecordsDto?>AddRecords(CreateRecordsDto records)
        {
            //check if patient and doctor exists in the system

            var patient = await context.Patients.FirstOrDefaultAsync(p => p.Id == records.PatientId);
            if (patient is null)
            {
                return null;
            }
            var doctor = await context.Users.FirstOrDefaultAsync(d => d.Id == records.DoctorId);
            if (doctor is null)
            {
                return null;
            }

            var record = new Records
            {
                Diagnosis = records.Diagnosis,
                Treatment = records.Treatment,
                PatientId = records.PatientId,
                DoctorId = records.DoctorId,
            };
            await context.Records.AddAsync(record);
            await context.SaveChangesAsync();
            return new RecordsDto
            {
                Id = record.Id,
                Diagnosis = record.Diagnosis,
                Treatment = record.Treatment,
                Patient = record.Patient == null ? null : new PatientsDto { Id = record.Patient.Id, FullName = record.Patient.FullName },
                Doctor = record.Doctor == null ? null : new UserDto { Id = record.Doctor.Id, FullName = record.Doctor.FullName, Email = record.Doctor.Email },
            };
        }


        public async Task<RecordsDto?> UpdateRecords(Guid id, CreateRecordsDto records)
        {
            var record = await context.Records.FindAsync(id);

            if (record is null)
            {
                return null;
            }

            record.Diagnosis = records.Diagnosis;
            record.Treatment = records.Treatment;
            record.PatientId = records.PatientId;
            record.DoctorId = records.DoctorId;
            await context.SaveChangesAsync();
            return new RecordsDto
            {
                Id = record.Id,
                Diagnosis = record.Diagnosis,
                Treatment = record.Treatment,
                Patient = record.Patient == null ? null : new PatientsDto { Id = record.Patient.Id, FullName = record.Patient.FullName },
                Doctor = record.Doctor == null ? null : new UserDto { Id = record.Doctor.Id, FullName = record.Doctor.FullName, Email = record.Doctor.Email },
            };
        }

        public async Task<bool> DeleteRecords(Guid id)
        {

            var record = await context.Records.FindAsync(id);
            if (record is null)
            {
                return false;
            }
            context.Records.Remove(record);
            await context.SaveChangesAsync();
            return true;
            
        }


    }
}