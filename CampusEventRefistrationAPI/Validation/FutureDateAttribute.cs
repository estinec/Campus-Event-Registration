using System.ComponentModel.DataAnnotations;

namespace CampusEventRefistrationAPI.Validation;

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        if (value is DateTime dateTime)
        {
            return dateTime > DateTime.UtcNow;
        }

        if (value is DateOnly dateOnly)
        {
            return dateOnly > DateOnly.FromDateTime(DateTime.UtcNow);
        }
        return false;
    }
}