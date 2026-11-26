// Name: Vishva Tyagi
// StudentId: 991811327

namespace EventRegistrationAPI_VishvaTyagi.DTOs;

/// <summary>
/// Output DTO for a registration. Includes the event title for display.
/// </summary>
public class RegistrationDto
{
    public int RegistrationId { get; set; }

    public string EventTitle { get; set; } = string.Empty;

    public string StudentName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime RegisteredOn { get; set; }
}
