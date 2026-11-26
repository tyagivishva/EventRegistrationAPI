// Name: Vishva Tyagi
// StudentId: 991811327

using System.ComponentModel.DataAnnotations;

namespace EventRegistrationAPI_VishvaTyagi.Validation;

/// <summary>
/// Validation attribute that rejects dates that are not in the future.
/// </summary>
public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not DateTime date)
        {
            return false;
        }

        return date > DateTime.Now;
    }
}
