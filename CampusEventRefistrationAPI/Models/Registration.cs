using System.ComponentModel.DataAnnotations;

namespace CampusEventRefistrationAPI.Models;

public class Registration
{
    public int RegistrationId { get; set; }
    
    public int  EventId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string StudentName { get; set; } = string.Empty;

    [Required]
    [EmailAddress(ErrorMessage = "Please provide a valid email address")]
    [MaxLength(50)]
    public string Email { get; set; } = "";
    
    public DateTime RegisteredOn { get; set; }

    public Event Event { get; set; } = null!;
}