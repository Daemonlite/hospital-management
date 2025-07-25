using Health.Data;
using Health.Entities;
using Health.Models;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;

namespace Health.services;

public class PatientsService(AppDbContext _context) : IPatientsService
{
    public async Task<List<Patients>> GetAllPatients()
    {
        return await _context.Patients.ToListAsync();
    }

    public async Task<Patients?> GetPatientById(Guid id)
    {
        return await _context.Patients.FindAsync(id);
    }

    public async Task<Patients?> AddPatient(PatientsDto patient)
    {
        if (await _context.Patients.AnyAsync(p => p.ContactInfo == patient.ContactInfo))
        {
            return null;
        }

        var newPatient = new Patients
        {
            FullName = patient.FullName,
            DOB = patient.DOB,
            Gender = patient.Gender,
            ContactInfo = patient.ContactInfo
        };

        await _context.Patients.AddAsync(newPatient);
        await _context.SaveChangesAsync();
        return newPatient;
    }

    public async Task<bool> DeletePatient(Guid id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient is null)
        {
            return false;
        }

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Patients?> UpdatePatient(Guid id,PatientsDto patient)
    {
        var existingPatient = await _context.Patients.FindAsync(id);
        if (existingPatient is null)
        {
            return null;
        }

        existingPatient.FullName = patient.FullName;
        existingPatient.DOB = patient.DOB;
        existingPatient.Gender = patient.Gender;
        existingPatient.ContactInfo = patient.ContactInfo;

        _context.Patients.Update(existingPatient);
        await _context.SaveChangesAsync();
        return existingPatient;
    }

}