// Name: Vishva Tyagi
// StudentId: 991811327

namespace EventRegistrationAPI_VishvaTyagi.Exceptions;

/// <summary>
/// Thrown by the registration service when a business rule is broken.
/// The controller maps StatusCode to the HTTP response.
/// </summary>
public class BusinessRuleException : Exception
{
    public int StatusCode { get; }

    public BusinessRuleException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
