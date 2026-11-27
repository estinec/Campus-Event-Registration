using CampusEventRefistrationAPI.DTOs;

namespace CampusEventRefistrationAPI.Services;

// Defines the contract for viewing, creating, and cancelling event registrations.
public interface IRegistrationService
{
    Task<List<RegistrationDto>?> GetRegistrationsForEventAsync(int eventId); // Lists registrations for an event, or null if the event does not exist
    Task<RegistrationDto> RegisterForEventAsync(int eventId, CreateRegistrationDto dto); // Enforces business rules and creates a registration
    Task<bool> CancelRegistrationAsync(int registrationId); // Deletes a registration by ID and returns whether a record was removed
}