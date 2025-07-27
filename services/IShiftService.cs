using Health.Entities;
using Health.Models;

namespace Health.services
{
    public interface IShiftService
    {
        public Task<List<ShiftDto>> GetAllShifts();
        public Task<List<ShiftDto>> GetShiftById(Guid id);

        public Task<List<ShiftDto>> GetAvailableSlotsByUserId(Guid userId, DateTime date);

        public Task<Shift> CreateShift(CreateShiftDto shiftDto);
        public Task<Shift> UpdateShift(Guid id, CreateShiftDto shiftDto);
        public Task<bool> DeleteShift(Guid id);
    }
}