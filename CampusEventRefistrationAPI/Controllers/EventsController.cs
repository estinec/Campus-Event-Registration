using Microsoft.AspNetCore.Mvc;
using CampusEventRefistrationAPI.DTOs;
using CampusEventRefistrationAPI.Services;

namespace CampusEventRefistrationAPI.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly IRegistrationService _registrationService;

    public EventsController(IEventService eventService, IRegistrationService registrationService)
    {
        _eventService = eventService;
        _registrationService = registrationService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EventDto>>> GetEvents()
    {
        var events = await _eventService.GetAllEventsAsync();
        return Ok(events);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventDto>> GetEvent(int id)
    {
        var eventDto = await _eventService.GetEventByIdAsync(id);
        return eventDto is null ? NotFound() : Ok(eventDto);
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto dto)
    {
        var created = await _eventService.CreateEventAsync(dto);
        return CreatedAtAction(nameof(GetEvent), new { id = created.EventId }, created);
    }

    [HttpGet("{id:int}/registrations")]
    public async Task<ActionResult<List<RegistrationDto>>> GetRegistrationsForEvent(int id)
    {
        var registrations = await _registrationService.GetRegistrationsForEventAsync(id);
        return registrations is null ? NotFound() : Ok(registrations);
    }

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
            return NotFound(ex.Message);
        }
        catch (BadRequestRuleException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ConflictRuleException ex)
        {
            return Conflict(ex.Message);
        }
    }
}