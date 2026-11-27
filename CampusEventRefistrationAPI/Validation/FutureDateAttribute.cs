using System.ComponentModel.DataAnnotations;

namespace CampusEventRefistrationAPI.Validation;

// Custom validation attribute that checks if a date is in the future.
public class FutureDateAttribute : ValidationAttribute
{
    // Returns true for null (handled by [Required]) or dates after the current UTC time.
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is DateTime dateTime)
        {
            return dateTime > DateTime.UtcNow; // Compare DateTime against current UTC time
        }

        if (value is DateOnly dateOnly)
        {
            return dateOnly > DateOnly.FromDateTime(DateTime.UtcNow); // Compare DateOnly against today's UTC date
        }

        return false; // Reject unsupported types
    }
}