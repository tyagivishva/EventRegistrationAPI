// Name: Vishva Tyagi
// StudentId: 991811327

using System.ComponentModel.DataAnnotations;

namespace EventRegistrationAPI_VishvaTyagi.Models;

/// <summary>
/// One student's registration for a campus event.
/// Many registrations belong to a single event.
/// </summary>
public class Registration
{
    public int RegistrationId { get; set; }

    public int EventId { get; set; }

    [Required]
    [MaxLength(100)]
    public string StudentName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    public DateTime RegisteredOn { get; set; }

    public Event Event { get; set; } = null!;
}
