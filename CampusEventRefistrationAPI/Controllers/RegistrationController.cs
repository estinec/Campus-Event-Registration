using Microsoft.AspNetCore.Mvc;
using CampusEventRefistrationAPI.Services;

namespace CampusEventRefistrationAPI.Controllers;

[ApiController]
[Route("api/registrations")]
public class RegistrationsController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public RegistrationsController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> CancelRegistration(int id)
    {
        var deleted = await _registrationService.CancelRegistrationAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}