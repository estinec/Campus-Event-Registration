using Microsoft.AspNetCore.Mvc;
using CampusEventRefistrationAPI.DTOs;
using CampusEventRefistrationAPI.Services;

namespace CampusEventRefistrationAPI.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService; // Event business logic
    private readonly IRegistrationService _registrationService; // Registration business logic

    public EventsController(IEventService eventService, IRegistrationService registrationService)
    {
        _eventService = eventService;
        _registrationService = registrationService;
    }

    // GET /api/events - returns all events sorted by date
    [HttpGet]
    public async Task<ActionResult<List<EventDto>>> GetEvents()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    // GET /api/events/{id} - returns a single event or 404
    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventDto>> GetEvent(int id)
    {
        var eventDto = await _eventService.GetEventByIdAsync(id);
        return eventDto is null ? NotFound() : Ok(eventDto);
    }

    // POST /api/events - creates an event and returns 201 Created
    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto dto)
    {
        var created = await _eventService.CreateEventAsync(dto);
        return CreatedAtAction(nameof(GetEvent), new { id = created.EventId }, created);
    }

    // GET /api/events/{id}/registrations - lists registrations for an event or 404
    [HttpGet("{id:int}/registrations")]
    public async Task<ActionResult<List<RegistrationDto>>> GetRegistrationsForEvent(int id)
    {
        var registrations = await _registrationService.GetRegistrationsForEventAsync(id);
        return registrations is null ? NotFound() : Ok(registrations);
    }

    //registers a student and maps rule errors to HTTP status codes
    [HttpPost("{id:int}/registrations")]
    public async Task<ActionResult<RegistrationDto>> RegisterForEvent(int id, [FromBody] CreateRegistrationDto dto)
    {
        try
        {
            var created = await _registrationService.RegisterForEventAsync(id, dto);
            return StatusCode(StatusCodes.Status201Created, created);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message); //404
        }
        catch (BadRequestRuleException ex)
        {
            return BadRequest(ex.Message); //400
        }
        catch (ConflictRuleException ex)
        {
            return Conflict(ex.Message); //409
        }
    }
}