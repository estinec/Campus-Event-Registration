using CampusEventRefistrationAPI.DTOs;

namespace CampusEventRefistrationAPI.Services;

// Defines the contract for event retrieval and creation operations.
public interface IEventService
{
    Task<List<EventDto>> GetAllEventsAsync(); // Retrieves all events ordered by date
    Task<EventDto?> GetEventByIdAsync(int id); // Retrieves a single event by ID, or null if not found
    Task<EventDto> CreateEventAsync(CreateEventDto dto); // Creates a new event and returns its DTO
}