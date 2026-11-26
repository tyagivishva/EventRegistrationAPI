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
/// Rules are checked in assignment order: event exists, not past, no duplicate email, capacity.
/// </summary>
public class RegistrationService : IRegistrationService
{
    private readonly AppDbContext _context;

    public RegistrationService(AppDbContext context)
    {
        _context = context;
    }

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

    public async Task<RegistrationDto> CreateAsync(int eventId, CreateRegistrationDto dto)
    {
        // Rule 1: Event must exist
        var campusEvent = await _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.EventId == eventId);

        if (campusEvent == null)
        {
            throw new BusinessRuleException("Event not found.", StatusCodes.Status404NotFound);
        }

        // Rule 2: No past events
        if (campusEvent.EventDate < DateTime.Now)
        {
            throw new BusinessRuleException(
                "Registration is closed for past events.",
                StatusCodes.Status400BadRequest);
        }

        // Rule 3: No duplicate email for this event (case-insensitive)
        var emailAlreadyRegistered = campusEvent.Registrations
            .Any(r => string.Equals(r.Email, dto.Email, StringComparison.OrdinalIgnoreCase));

        if (emailAlreadyRegistered)
        {
            throw new BusinessRuleException(
                "This email is already registered for this event.",
                StatusCodes.Status409Conflict);
        }

        // Rule 4: Capacity — event is full when registration count equals capacity
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

    public async Task<bool> DeleteAsync(int id)
    {
        var registration = await _context.Registrations.FindAsync(id);

        if (registration == null)
        {
            return false;
        }

        // Cancelling a registration frees a spot for later registrations.
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
