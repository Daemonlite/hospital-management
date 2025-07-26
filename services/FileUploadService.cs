using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Health.Data;
using Health.Models;
using Health.Entities;
using Health.services;

namespace Health.services
{
    public class FileUploadService(AppDbContext _context, IWebHostEnvironment _env)
    {

        public async Task<PatientsFiles> UploadPatientsFileAsync(Guid patientId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            var user = await _context.Patients.FindAsync(patientId) ?? throw new Exception("Patient not found");
            var uploadsPath = Path.Combine(_env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot"), "uploads", "patients", patientId.ToString());

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var patientsFile = new PatientsFiles
            {
                PatientId = user.Id,
                FileName = file.FileName,
                FilePath = $"/uploads/patients/{patientId}/{fileName}",
                ContentType = file.ContentType,
                FileSize = file.Length
            };

            _context.PatientsFiles.Add(patientsFile);
            await _context.SaveChangesAsync();

            return patientsFile;
        }


    }
}
