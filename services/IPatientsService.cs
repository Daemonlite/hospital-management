using Health.Entities;
using Health.Models;

namespace Health.services
{
    public interface IPatientsService
    {
        Task<List<Patients>> GetAllPatients();
        Task<Patients?> GetPatientById(Guid id);
        Task<Patients?> AddPatient(PatientsCreateDto patient);
        Task<Patients?> UpdatePatient(Guid id,PatientsCreateDto patient);
        Task<bool> DeletePatient(Guid id);
    }
}