// Name: Vishva Tyagi
// StudentId: 991811327

using EventRegistrationAPI_VishvaTyagi.DTOs;
using EventRegistrationAPI_VishvaTyagi.Exceptions;
using EventRegistrationAPI_VishvaTyagi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistrationAPI_VishvaTyagi.Controllers;

/// <summary>
/// Registration endpoints and DELETE. Controllers call IRegistrationService only —
/// never the DbContext.
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
    /// GET /api/events/{eventId}/registrations
    /// Returns 200 with registrations, or 404 if the event does not exist.
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
    /// POST /api/events/{eventId}/registrations
    /// Returns 201 with RegistrationDto on success.
    /// Maps business-rule failures to 404, 400, or 409 with the assignment messages.
    /// </summary>
    [HttpPost("events/{eventId:int}/registrations")]
    public async Task<IActionResult> Create(int eventId, CreateRegistrationDto dto)
    {
        try
        {
            var registration = await _registrationService.CreateAsync(eventId, dto);
            return StatusCode(StatusCodes.Status201Created, registration);
        }
        catch (BusinessRuleException ex) when (ex.StatusCode == StatusCodes.Status404NotFound)
        {
            return NotFound();
        }
        catch (BusinessRuleException ex) when (ex.StatusCode == StatusCodes.Status400BadRequest)
        {
            return BadRequest(ex.Message);
        }
        catch (BusinessRuleException ex) when (ex.StatusCode == StatusCodes.Status409Conflict)
        {
            return Conflict(ex.Message);
        }
    }

    /// <summary>
    /// DELETE /api/registrations/{id}
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
