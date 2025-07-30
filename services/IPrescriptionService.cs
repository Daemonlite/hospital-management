using Health.Models;

namespace Health.services
{
    public interface IPrescriptionService
    {
        Task<PrescriptionsCreateDto?> CreatePrescription(PrescriptionsCreateDto prescriptionsDto);
        Task<List<PrescriptionListDto>> GetAllPrescriptions();
        Task<List<PrescriptionListDto>> GetPrescriptionById(Guid id);
        Task<List<PrescriptionListDto>> GetPrescriptionByPatientId(Guid id);
        Task<List<PrescriptionListDto>> GetPrescriptionByDoctorId(Guid id);
        Task<PrescriptionsCreateDto?> UpdatePrescription(Guid id,PrescriptionsCreateDto prescriptionsDto);
        Task<bool> DeletePrescription(Guid id);
    }
}