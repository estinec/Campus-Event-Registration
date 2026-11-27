using Microsoft.EntityFrameworkCore;
using CampusEventRefistrationAPI.Data;
using CampusEventRefistrationAPI.DTOs;
using CampusEventRefistrationAPI.Models;

namespace CampusEventRefistrationAPI.Services;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class BadRequestRuleException : Exception
{
    public BadRequestRuleException(string message) : base(message) { }
}

public class ConflictRuleException : Exception
{
    public ConflictRuleException(string message) : base(message) { }
}

public class RegistrationService : IRegistrationService
{
    private readonly AppDbContext _db;

    public RegistrationService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<RegistrationDto>?> GetRegistrationsForEventAsync(int eventId)
    {
        var eventExists = await _db.Events.AnyAsync(e => e.EventId == eventId);
        if (!eventExists)
        {
            return null;
        }

        return await _db.Registrations
            .Where(r => r.EventId == eventId)
            .OrderBy(r => r.RegisteredOn)
            .Select(r => new RegistrationDto
            {
                RegistrationId = r.RegistrationId,
                EventTitle = r.Event.Title,
                StudentName = r.StudentName,
                Email = r.Email,
                RegisteredOn = r.RegisteredOn
            })
            .ToListAsync();
    }

    public async Task<RegistrationDto> RegisterForEventAsync(int eventId, CreateRegistrationDto dto)
    {
        var eventEntity = await _db.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.EventId == eventId);

        // 1. Event must exist -> 404 Not Found
        if (eventEntity is null)
        {
            throw new NotFoundException($"Event with ID {eventId} was not found.");
        }

        // 2. No past events -> 400: "Registration is closed for past events."
        if (eventEntity.EventDate <= DateTime.UtcNow)
        {
            throw new BadRequestRuleException("Registration is closed for past events.");
        }

        // 3. No duplicates (case-insensitive) -> 409 Conflict: "This email is already registered for this event."
        var normalizedEmail = dto.Email.Trim();
        var alreadyRegistered = eventEntity.Registrations
            .Any(r => string.Equals(r.Email.Trim(), normalizedEmail, StringComparison.OrdinalIgnoreCase));

        if (alreadyRegistered)
        {
            throw new ConflictRuleException("This email is already registered for this event.");
        }

        // 4. Capacity -> 409 Conflict: "This event is full."
        if (eventEntity.Registrations.Count >= eventEntity.Capacity)
        {
            throw new ConflictRuleException("This event is full.");
        }

        var registration = new Registration
        {
            EventId = eventId,
            StudentName = dto.StudentName.Trim(),
            Email = normalizedEmail,
            RegisteredOn = DateTime.UtcNow
        };

        _db.Registrations.Add(registration);
        await _db.SaveChangesAsync();

        return new RegistrationDto
        {
            RegistrationId = registration.RegistrationId,
            EventTitle = eventEntity.Title,
            StudentName = registration.StudentName,
            Email = registration.Email,
            RegisteredOn = registration.RegisteredOn
        };
    }

    public async Task<bool> CancelRegistrationAsync(int registrationId)
    {
        var registration = await _db.Registrations
            .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);

        if (registration is null)
        {
            return false;
        }

        _db.Registrations.Remove(registration);
        await _db.SaveChangesAsync();
        return true;
    }
}