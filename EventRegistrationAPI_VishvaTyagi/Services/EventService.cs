// Name: Vishva Tyagi
// StudentId: 991811327

using EventRegistrationAPI_VishvaTyagi.Data;
using EventRegistrationAPI_VishvaTyagi.DTOs;
using EventRegistrationAPI_VishvaTyagi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationAPI_VishvaTyagi.Services;

/// <summary>
/// Handles database access and mapping for events.
/// RegisteredCount and SpotsLeft are calculated from the registration collection.
/// </summary>
public class EventService : IEventService
{
    private readonly AppDbContext _context;

    public EventService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EventDto>> GetAllAsync()
    {
        var events = await _context.Events
            .Include(e => e.Registrations)
            .OrderBy(e => e.EventDate)
            .ToListAsync();

        return events.Select(MapToDto);
    }

    public async Task<EventDto?> GetByIdAsync(int id)
    {
        var campusEvent = await _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.EventId == id);

        return campusEvent == null ? null : MapToDto(campusEvent);
    }

    private static EventDto MapToDto(Event campusEvent)
    {
        var registeredCount = campusEvent.Registrations.Count;

        return new EventDto
        {
            EventId = campusEvent.EventId,
            Title = campusEvent.Title,
            Location = campusEvent.Location,
            EventDate = campusEvent.EventDate,
            Capacity = campusEvent.Capacity,
            RegisteredCount = registeredCount,
            SpotsLeft = campusEvent.Capacity - registeredCount
        };
    }
}
