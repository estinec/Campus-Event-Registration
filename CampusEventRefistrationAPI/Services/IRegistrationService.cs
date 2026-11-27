using CampusEventRefistrationAPI.DTOs;

namespace CampusEventRefistrationAPI.Services;

public interface IRegistrationService
{
    Task<List<RegistrationDto>?> GetRegistrationsForEventAsync(int eventId);
    Task<RegistrationDto> RegisterForEventAsync(int eventId, CreateRegistrationDto dto);
    Task<bool> CancelRegistrationAsync(int registrationId);
}