// Name: Vishva Tyagi
// StudentId: 991811327

using EventRegistrationAPI_VishvaTyagi.Data;
using EventRegistrationAPI_VishvaTyagi.DTOs;
using EventRegistrationAPI_VishvaTyagi.Exceptions;
using EventRegistrationAPI_VishvaTyagi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationAPI_VishvaTyagi.Services;

/// <summary>
/// Handles registration database work, DTO mapping, and business rules.
/// Rules are checked in assignment order so a duplicate email gets the duplicate
/// message even when the event is also full.
/// </summary>
public class RegistrationService : IRegistrationService
{
    private readonly AppDbContext _context;

    public RegistrationService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns registrations for an event, or null when the event id does not exist.
    /// </summary>
    public async Task<IEnumerable<RegistrationDto>?> GetByEventIdAsync(int eventId)
    {
        var campusEvent = await _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.EventId == eventId);

        if (campusEvent == null)
        {
            return null;
        }

        return campusEvent.Registrations.Select(r => MapToDto(r, campusEvent.Title));
    }

    /// <summary>
    /// Registers a student for an event after enforcing the four business rules.
    /// RegisteredOn is set here automatically — it is not part of CreateRegistrationDto.
    /// </summary>
    public async Task<RegistrationDto> CreateAsync(int eventId, CreateRegistrationDto dto)
    {
        // Rule 1: Event must exist → 404
        var campusEvent = await _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.EventId == eventId);

        if (campusEvent == null)
        {
            throw new BusinessRuleException("Event not found.", StatusCodes.Status404NotFound);
        }

        // Rule 2: No past events → 400
        if (campusEvent.EventDate < DateTime.Now)
        {
            throw new BusinessRuleException(
                "Registration is closed for past events.",
                StatusCodes.Status400BadRequest);
        }

        // Rule 3: No duplicate email for this event (case-insensitive) → 409
        // Checked before capacity so a full event with a duplicate still returns the duplicate message.
        var emailAlreadyRegistered = campusEvent.Registrations
            .Any(r => string.Equals(r.Email, dto.Email, StringComparison.OrdinalIgnoreCase));

        if (emailAlreadyRegistered)
        {
            throw new BusinessRuleException(
                "This email is already registered for this event.",
                StatusCodes.Status409Conflict);
        }

        // Rule 4: Capacity — event is full when registration count equals capacity → 409
        if (campusEvent.Registrations.Count >= campusEvent.Capacity)
        {
            throw new BusinessRuleException(
                "This event is full.",
                StatusCodes.Status409Conflict);
        }

        var registration = new Registration
        {
            EventId = eventId,
            StudentName = dto.StudentName,
            Email = dto.Email,
            RegisteredOn = DateTime.Now
        };

        _context.Registrations.Add(registration);
        await _context.SaveChangesAsync();

        return MapToDto(registration, campusEvent.Title);
    }

    /// <summary>
    /// Cancels a registration. Deleting frees a spot so SpotsLeft goes up by one.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var registration = await _context.Registrations.FindAsync(id);

        if (registration == null)
        {
            return false;
        }

        _context.Registrations.Remove(registration);
        await _context.SaveChangesAsync();
        return true;
    }

    private static RegistrationDto MapToDto(Registration registration, string eventTitle)
    {
        return new RegistrationDto
        {
            RegistrationId = registration.RegistrationId,
            EventTitle = eventTitle,
            StudentName = registration.StudentName,
            Email = registration.Email,
            RegisteredOn = registration.RegisteredOn
        };
    }
}
