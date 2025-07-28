using Health.Models;

namespace Health.services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDto>> GetAppointments();
        Task<List<AppointmentDto>> GetAppointmentsByPatientId(Guid id);
        Task<List<AppointmentDto>> GetAppointmentsByUserId(Guid id);
        Task<AppointmentDto?> GetAppointmentById(Guid id);
        Task<CreateAppointmentDto> CreateAppointment(CreateAppointmentDto appointmentDto);
        Task<CreateAppointmentDto?> UpdateAppointment(Guid id, CreateAppointmentDto appointmentDto);
        Task<bool> DeleteAppointment(Guid id);
    }
}