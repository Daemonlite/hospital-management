

using Health.Data;
using Health.Entities;
using Health.Models;
using Microsoft.EntityFrameworkCore;

namespace Health.services
{
    public class ShiftServices(AppDbContext context) : IShiftService
    {

        public async Task<List<ShiftDto>> GetAllShifts()
        {
            IQueryable<Shift> query = context.Shifts
                .Include(s => s.User)
                .AsQueryable();

            return await query
                .OrderBy(s => s.StartTime)
                .Select(s => new ShiftDto
                {
                    Id = s.Id,
                    User = new UserDto { Id = s.User.Id, FullName = s.User.FullName,Email = s.User.Email },
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsBooked = s.IsBooked,
                    Recurrence = s.Recurrence.ToString(),
                    Notes = s.Notes
                })
                .ToListAsync();
        }

        public async Task<List<ShiftDto>> GetShiftById(Guid id)
        {
            return await context.Shifts
                .Where(s => s.Id == id)
                .Select(s => new ShiftDto
                {
                    Id = s.Id,
                    User = new UserDto { Id = s.User.Id, FullName = s.User.FullName, Email = s.User.Email },
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsBooked = s.IsBooked,
                    Recurrence = s.Recurrence.ToString(),
                    Notes = s.Notes
                })
                .ToListAsync() ?? [];
        } 

        public async Task<List<ShiftDto>> GetAvailableSlotsByUserId(Guid userId, DateTime date)
        {
            return await context.Shifts
            .Where(s => s.UserId == userId &&
                       s.StartTime.Date == date.Date &&
                       !s.IsBooked)
            .Select(s => new ShiftDto
            {
                Id = s.Id,
                User = new UserDto { Id = s.User.Id, FullName = s.User.FullName },
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsBooked = s.IsBooked,
                Recurrence = s.Recurrence.ToString(),
                Notes = s.Notes
            })
            .ToListAsync() ?? [];

        }


        public async Task<Shift> CreateShift(CreateShiftDto shiftDto)
        {
            // Check for overlapping shifts
            var hasConflict = await context.Shifts
                .AnyAsync(s => s.UserId == shiftDto.UserId &&
                              s.StartTime < shiftDto.EndTime &&
                              s.EndTime > shiftDto.StartTime);

            if (hasConflict)
            {
                throw new InvalidOperationException("Shift conflicts with existing schedule");
            }
            

            var shift = new Shift
            {
                User = await context.Users.FindAsync(shiftDto.UserId) ?? throw new KeyNotFoundException("User not found"),
                UserId = shiftDto.UserId,
                StartTime = shiftDto.StartTime,
                EndTime = shiftDto.EndTime,
                Recurrence = Enum.Parse<RecurrencePattern>(shiftDto.Recurrence),
                Notes = shiftDto.Notes,
                IsBooked = false
            };

            await context.Shifts.AddAsync(shift);
            await context.SaveChangesAsync();

            if (Enum.Parse<RecurrencePattern>(shiftDto.Recurrence) != RecurrencePattern.None)
            {
                await GenerateRecurringShifts(shift.Id);
            }

            return shift;
        }


        public async Task<Shift> UpdateShift(Guid id, CreateShiftDto shiftDto)
        {
            var existingShift = await context.Shifts.FindAsync(id);
            if (existingShift == null)
            {
                throw new KeyNotFoundException("Shift not found");
            }

            // Validate time range
            if (shiftDto.EndTime <= shiftDto.StartTime)
            {
                throw new ArgumentException("End time must be after start time");
            }

            // Check for conflicts (excluding self)
            var hasConflict = await context.Shifts
                .AnyAsync(s => s.Id != id &&
                              s.UserId == shiftDto.UserId &&
                              s.StartTime < shiftDto.EndTime &&
                              s.EndTime > shiftDto.StartTime);

            if (hasConflict)
            {
                throw new InvalidOperationException("Updated shift conflicts with existing schedule");
            }

            existingShift.StartTime = shiftDto.StartTime;
            existingShift.EndTime = shiftDto.EndTime;
            existingShift.Recurrence = Enum.Parse<RecurrencePattern>(shiftDto.Recurrence);
            existingShift.Notes = shiftDto.Notes;

            

            await context.SaveChangesAsync();
            return existingShift;
        }

        public async Task<bool> DeleteShift(Guid id)
        {
            var shift = await context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return false;
            }

            // Prevent deletion of booked shifts
            if (shift.IsBooked)
            {
                throw new InvalidOperationException("Cannot delete a booked shift");
            }

            context.Shifts.Remove(shift);
            await context.SaveChangesAsync();
            return true;
        }
        
        // Additional helper method for generating recurring shifts
        public async Task GenerateRecurringShifts(Guid shiftId)
        {
            var baseShift = await context.Shifts.FindAsync(shiftId);
            if (baseShift == null || baseShift.Recurrence == RecurrencePattern.None)
            {
                return;
            }

            var currentDate = baseShift.StartTime;
            var endDate = DateTime.UtcNow.AddMonths(3); // Generate 3 months ahead

            while (currentDate <= endDate)
            {
                currentDate = baseShift.Recurrence switch
                {
                    RecurrencePattern.Daily => currentDate.AddDays(1),
                    RecurrencePattern.Weekly => currentDate.AddDays(7),
                    RecurrencePattern.BiWeekly => currentDate.AddDays(14),
                    RecurrencePattern.Monthly => currentDate.AddMonths(1),
                    _ => endDate.AddDays(1) // Exit loop
                };

                if (currentDate > endDate) break;

                var newShift = new Shift
                {
                    UserId = baseShift.UserId,
                    User = baseShift.User,
                    StartTime = new DateTime(
                        currentDate.Year,
                        currentDate.Month,
                        currentDate.Day,
                        baseShift.StartTime.Hour,
                        baseShift.StartTime.Minute,
                        baseShift.StartTime.Second),
                    EndTime = new DateTime(
                        currentDate.Year,
                        currentDate.Month,
                        currentDate.Day,
                        baseShift.EndTime.Hour,
                        baseShift.EndTime.Minute,
                        baseShift.EndTime.Second),
                    Recurrence = RecurrencePattern.None, // Individual instances
                    IsBooked = false,
                    Notes = $"Recurring shift from {baseShift.Id}"
                };

                await context.Shifts.AddAsync(newShift);
            }

            await context.SaveChangesAsync();
        }
    }
}