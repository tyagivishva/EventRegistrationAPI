using System.ComponentModel.DataAnnotations;

namespace EventRegistrationAPI_VishvaTyagi.Models;

/// <summary>
/// A campus event students and staff can register for.
/// One event has many registrations.
/// </summary>
public class Event
{
    public int EventId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Location { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    public int Capacity { get; set; }

    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
