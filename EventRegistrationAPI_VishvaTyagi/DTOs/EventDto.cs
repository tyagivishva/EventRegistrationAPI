// Name: Vishva Tyagi
// StudentId: 991811327

namespace EventRegistrationAPI_VishvaTyagi.DTOs;

/// <summary>
/// Output DTO for an event. RegisteredCount and SpotsLeft are calculated, not stored.
/// </summary>
public class EventDto
{
    public int EventId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    public int Capacity { get; set; }

    public int RegisteredCount { get; set; }

    public int SpotsLeft { get; set; }
}
