using Health.Entities;
using Health.Models;

namespace Health.services
{
    public interface IFileUploadService
    {
        Task<PatientsFiles> UploadPatientsFileAsync(Guid userId, IFormFile file);
    }
}
