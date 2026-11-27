using Microsoft.EntityFrameworkCore;
using CampusEventRefistrationAPI.Data;
using CampusEventRefistrationAPI.DTOs;
using CampusEventRefistrationAPI.Models;

namespace CampusEventRefistrationAPI.Services;

// Service handling event queries and creation, mapping Event entities to EventDto responses.
public class EventService : IEventService
{
    private readonly AppDbContext _db; // Database context for accessing events and registrations

    public EventService(AppDbContext db)
    {
        _db = db;
    }

    // Returns all events ordered chronologically by event date with calculated spot counts.
    public async Task<List<EventDto>> GetAllEventsAsync()
    {
        return await _db.Events
            .Include(e => e.Registrations)
            .OrderBy(e => e.EventDate)
            .Select(e => ToDto(e))
            .ToListAsync();
    }

    // Returns a single event DTO by ID, or null if no matching event exists.
    public async Task<EventDto?> GetEventByIdAsync(int id)
    {
        var eventEntity = await _db.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.EventId == id);

        return eventEntity is null ? null : ToDto(eventEntity);
    }

    // Validates/trims incoming event data, saves it to the database, and returns the created DTO.
    public async Task<EventDto> CreateEventAsync(CreateEventDto dto)
    {
        var eventEntity = new Event
        {
            Title = dto.Title.Trim(),
            Location = dto.Location.Trim(),
            EventDate = dto.EventDate,
            Capacity = dto.Capacity
        };

        _db.Events.Add(eventEntity);
        await _db.SaveChangesAsync();

        return ToDto(eventEntity);
    }

    // Converts an Event entity into an EventDto and computes RegisteredCount and SpotsLeft.
    private static EventDto ToDto(Event eventEntity)
    {
        var registeredCount = eventEntity.Registrations?.Count ?? 0; // Current registration count (0 for newly created events)

        return new EventDto
        {
            EventId = eventEntity.EventId,
            Title = eventEntity.Title,
            Location = eventEntity.Location,
            EventDate = eventEntity.EventDate,
            Capacity = eventEntity.Capacity,
            RegisteredCount = registeredCount,
            SpotsLeft = Math.Max(0, eventEntity.Capacity - registeredCount) // Prevent negative spots if capacity was ever exceeded
        };
    }
}