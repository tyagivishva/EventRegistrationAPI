// Name: Vishva Tyagi
// StudentId: 991811327

using EventRegistrationAPI_VishvaTyagi.DTOs;
using EventRegistrationAPI_VishvaTyagi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventRegistrationAPI_VishvaTyagi.Controllers;

/// <summary>
/// API endpoints for campus events. All data access goes through IEventService.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    /// <summary>
    /// Returns all events as DTOs, sorted by event date.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var events = await _eventService.GetAllAsync();
        return Ok(events);
    }

    /// <summary>
    /// Returns one event by id, or 404 if it does not exist.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var campusEvent = await _eventService.GetByIdAsync(id);

        if (campusEvent == null)
        {
            return NotFound();
        }

        return Ok(campusEvent);
    }

    /// <summary>
    /// Creates a new event from CreateEventDto. Invalid input returns 400 automatically.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateEventDto dto)
    {
        var createdEvent = await _eventService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdEvent.EventId }, createdEvent);
    }
}
