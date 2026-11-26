// Name: Vishva Tyagi
// StudentId: 991811327

using EventRegistrationAPI_VishvaTyagi.DTOs;

namespace EventRegistrationAPI_VishvaTyagi.Services;

/// <summary>
/// Service contract for registration operations and business rules.
/// Controllers call this interface instead of using the DbContext directly.
/// </summary>
public interface IRegistrationService
{
    Task<IEnumerable<RegistrationDto>?> GetByEventIdAsync(int eventId);

    Task<RegistrationDto> CreateAsync(int eventId, CreateRegistrationDto dto);

    Task<bool> DeleteAsync(int id);
}
