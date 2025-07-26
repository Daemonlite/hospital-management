using Health.Entities;
using Health.Models;

namespace Health.services
{
    public interface IPatientFilesService
    {
        Task<List<SinglePatientFileDto>?> GetPatientFilesByPatientId(Guid id);

        Task<List<PatientFilesListDto>?> FetchAllFiles();

        Task<PatientsFiles> AddPatientFile(PatientFilesDto patientFile);

        Task<bool> DeletePatientFile(Guid id);

    }
}