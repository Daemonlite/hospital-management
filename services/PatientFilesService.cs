using System.Collections.Immutable;
using Health.Data;
using Health.Entities;
using Health.Models;
using Microsoft.EntityFrameworkCore;

namespace Health.services
{
    public class PatientFileService(AppDbContext context, FileUploadService fileUploadService) : IPatientFilesService
    {
        public async Task<List<PatientFilesListDto>?>FetchAllFiles()
        {
            var files = await context.PatientsFiles
                .Include(f => f.Patient)
                .ToListAsync();

            return [.. files.Select(f => new PatientFilesListDto
            {
                Id = f.Id,
                Description = f.Description,
                FilePath = f.FilePath,
                FileSize = f.FileSize,
                UploadedAt = f.UploadedAt,
                ContentType = f.ContentType,
                FileName = f.FileName,
                Patient = f.Patient != null ? new Patients
                {
                    Id = f.Patient.Id,
                    FullName = f.Patient.FullName,
                    DOB = f.Patient.DOB,
                    Gender = f.Patient.Gender
                } : null
            })];
        }

        public async Task<List<SinglePatientFileDto>?> GetPatientFilesByPatientId(Guid id)
        {
            var patientExists = await context.Patients.AnyAsync(p => p.Id == id);
            if (!patientExists) return null;

            var files = await context.PatientsFiles
                .Where(f => f.PatientId == id)
                .ToListAsync();

            if (files.Count == 0) return null;

            return [.. files.Select(f => new SinglePatientFileDto
            {
                Id = f.Id,
                Description = f.Description,
                FileName = f.FileName,
                FilePath = f.FilePath,
                ContentType = f.ContentType,
                FileSize = f.FileSize,
                UploadedAt = f.UploadedAt,
            })];
        }

        public async Task<PatientsFiles?> AddPatientFile(PatientFilesDto patientFile)
        {
            var patientExists = await context.Patients.AnyAsync(p => p.Id == patientFile.PatientId);
            if (!patientExists)
            {
                throw new ArgumentException("Patient does not exist");
            }
            ;

            var upload = await fileUploadService.UploadPatientsFileAsync(patientId: patientFile.PatientId, file: patientFile.File, uploadLocation: "patients");

            var newFile = new PatientsFiles
            {
                Id = Guid.NewGuid(),
                PatientId = patientFile.PatientId,
                Description = patientFile.Description,
                FileName = upload.FileName,
                FilePath = upload.AbsoluteUrl,
                ContentType = upload.ContentType,
                FileSize = upload.FileSize,
                UploadedAt = DateTime.UtcNow
            };
            
            await context.PatientsFiles.AddAsync(newFile);
            await context.SaveChangesAsync();
            return newFile;

        }

        public async Task<bool> DeletePatientFile(Guid id)
        {
            var file = await context.PatientsFiles.FindAsync(id);
            if (file is null)
            {
                return false;
            }
            context.PatientsFiles.Remove(file);
            await context.SaveChangesAsync();
            return true;
        }
    }
}