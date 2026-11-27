using Microsoft.EntityFrameworkCore;
using CampusEventRefistrationAPI.Data;
using CampusEventRefistrationAPI.DTOs;
using CampusEventRefistrationAPI.Models;

namespace CampusEventRefistrationAPI.Services;

public class EventService : IEventService
{
    private readonly AppDbContext _db;

    public EventService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<EventDto>> GetAllEventsAsync()
    {
        return await _db.Events
            .Include(e => e.Registrations)
            .OrderBy(e => e.EventDate)
            .Select(e => ToDto(e))
            .ToListAsync();
    }

    public async Task<EventDto?> GetEventByIdAsync(int id)
    {
        var eventEntity = await _db.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.EventId == id);

        return eventEntity is null ? null : ToDto(eventEntity);
    }

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

    private static EventDto ToDto(Event eventEntity)
    {
        var registeredCount = eventEntity.Registrations?.Count ?? 0;

        return new EventDto
        {
            EventId = eventEntity.EventId,
            Title = eventEntity.Title,
            Location = eventEntity.Location,
            EventDate = eventEntity.EventDate,
            Capacity = eventEntity.Capacity,
            RegisteredCount = registeredCount,
            SpotsLeft = Math.Max(0, eventEntity.Capacity - registeredCount)
        };
    }
}