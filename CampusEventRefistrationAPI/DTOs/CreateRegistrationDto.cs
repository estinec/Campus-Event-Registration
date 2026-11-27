using System.ComponentModel.DataAnnotations;

namespace CampusEventRefistrationAPI.DTOs;

// Input data sent by a client to register a student for an event with validations
public class CreateRegistrationDto
{
    [Required(ErrorMessage = "Student name is required.")]
    [StringLength(50, ErrorMessage = "Student name must be at most 50 characters.")]
    public string StudentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
    [StringLength(50, ErrorMessage = "Email must be at most 50 characters.")]
    public string Email { get; set; } = string.Empty;
}