// Name: Vishva Tyagi
// StudentId: 991811327

using EventRegistrationAPI_VishvaTyagi.DTOs;
using EventRegistrationAPI_VishvaTyagi.Exceptions;
using EventRegistrationAPI_VishvaTyagi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistrationAPI_VishvaTyagi.Controllers;

/// <summary>
/// Registration endpoints and DELETE. Controllers call IRegistrationService only.
/// </summary>
[ApiController]
[Route("api")]
public class RegistrationsController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public RegistrationsController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    /// <summary>
    /// Returns all registrations for one event, or 404 if the event does not exist.
    /// </summary>
    [HttpGet("events/{eventId:int}/registrations")]
    public async Task<IActionResult> GetByEventId(int eventId)
    {
        var registrations = await _registrationService.GetByEventIdAsync(eventId);

        if (registrations == null)
        {
            return NotFound();
        }

        return Ok(registrations);
    }

    /// <summary>
    /// Registers a student for an event. Business-rule failures return 400, 404, or 409.
    /// </summary>
    [HttpPost("events/{eventId:int}/registrations")]
    public async Task<IActionResult> Create(int eventId, CreateRegistrationDto dto)
    {
        try
        {
            var registration = await _registrationService.CreateAsync(eventId, dto);
            return StatusCode(StatusCodes.Status201Created, registration);
        }
        catch (BusinessRuleException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
    }

    /// <summary>
    /// Cancels a registration and frees a spot. Returns 204, or 404 if not found.
    /// </summary>
    [HttpDelete("registrations/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _registrationService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
