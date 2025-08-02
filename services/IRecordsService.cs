
using Health.Models;

namespace Health.services
{
    public interface IRecordsService
    {
        Task<List<RecordsDto>> GetAllRecords();
        Task<RecordsDto?> GetRecordsById(Guid id);

        Task<List<RecordsDto>> GetRecordsByPatientId(Guid id);
        Task<RecordsDto?> AddRecords(CreateRecordsDto records);
        Task<RecordsDto?> UpdateRecords(Guid id,CreateRecordsDto records);
        Task<bool> DeleteRecords(Guid id);
    }
}