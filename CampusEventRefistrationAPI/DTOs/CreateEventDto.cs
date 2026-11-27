using System.ComponentModel.DataAnnotations;
using CampusEventRefistrationAPI.Validation;

namespace CampusEventRefistrationAPI.DTOs;

public class CreateEventDto
{
    [Required(ErrorMessage = "Event title is required.")]
    [StringLength(100, ErrorMessage = "Title must be at most 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Event location is required.")]
    [StringLength(50, ErrorMessage = "Location must be at most 50 characters.")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Event date is required.")]
    [FutureDate(ErrorMessage = "Event date must be in the future.")]
    public DateTime EventDate { get; set; }

    [Range(1, 500, ErrorMessage = "Capacity must be between 1 and 500.")]
    public int Capacity { get; set; }
}