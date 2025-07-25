using Health.Entities;
using Health.Models;

namespace Health.services
{
    public interface IPatientsService
    {
        Task<List<Patients>> GetAllPatients();
        Task<Patients?> GetPatientById(Guid id);
        Task<Patients?> AddPatient(PatientsDto patient);
        Task<Patients?> UpdatePatient(Guid id,PatientsDto patient);
        Task<bool> DeletePatient(Guid id);
    }
}