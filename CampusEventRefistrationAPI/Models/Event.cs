using System.ComponentModel.DataAnnotations;

namespace CampusEventRefistrationAPI.Models;

public class Event
{
    public int EventId { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "Length should be no more than 100 characters")]
    public string Title { get; set; } = String.Empty;
    [Required]
    [MaxLength(50)]
    public string Location { get; set; } = String.Empty;
    
    public DateTime EventDate { get; set; }
    
    public int Capacity { get; set; }
    
    public ICollection<Registration> Registrations { get; set; }
}