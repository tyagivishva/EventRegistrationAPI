// Name: Vishva Tyagi
// StudentId: 991811327

using EventRegistrationAPI_VishvaTyagi.DTOs;

namespace EventRegistrationAPI_VishvaTyagi.Services;

/// <summary>
/// Service contract for campus event operations.
/// Controllers call this interface instead of using the DbContext directly.
/// </summary>
public interface IEventService
{
    Task<IEnumerable<EventDto>> GetAllAsync();

    Task<EventDto?> GetByIdAsync(int id);

    Task<EventDto> CreateAsync(CreateEventDto dto);
}
