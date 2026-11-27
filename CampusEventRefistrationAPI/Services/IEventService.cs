using CampusEventRefistrationAPI.DTOs;

namespace CampusEventRefistrationAPI.Services;

public interface IEventService
{
    Task<List<EventDto>> GetAllEventsAsync();
    Task<EventDto?> GetEventByIdAsync(int id);
    Task<EventDto> CreateEventAsync(CreateEventDto dto);
}