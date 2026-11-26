// Name: Vishva Tyagi
// StudentId: 991811327

using EventRegistrationAPI_VishvaTyagi.Data;
using EventRegistrationAPI_VishvaTyagi.DTOs;
using EventRegistrationAPI_VishvaTyagi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationAPI_VishvaTyagi.Services;

/// <summary>
/// Handles database access and mapping for events.
/// RegisteredCount and SpotsLeft are calculated values — they are not stored in the database.
/// </summary>
public class EventService : IEventService
{
    private readonly AppDbContext _context;

    public EventService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns every event as an EventDto, sorted by EventDate ascending.
    /// </summary>
    public async Task<IEnumerable<EventDto>> GetAllAsync()
    {
        var events = await _context.Events
            .Include(e => e.Registrations)
            .OrderBy(e => e.EventDate)
            .ToListAsync();

        return events.Select(MapToDto);
    }

    /// <summary>
    /// Returns one event by id, or null when it does not exist.
    /// </summary>
    public async Task<EventDto?> GetByIdAsync(int id)
    {
        var campusEvent = await _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.EventId == id);

        return campusEvent == null ? null : MapToDto(campusEvent);
    }

    /// <summary>
    /// Creates a new event from CreateEventDto and returns the mapped EventDto.
    /// </summary>
    public async Task<EventDto> CreateAsync(CreateEventDto dto)
    {
        var campusEvent = new Event
        {
            Title = dto.Title,
            Location = dto.Location,
            EventDate = dto.EventDate,
            Capacity = dto.Capacity
        };

        _context.Events.Add(campusEvent);
        await _context.SaveChangesAsync();

        return MapToDto(campusEvent);
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
