// Name: Vishva Tyagi
// StudentId: 991811327

using System.ComponentModel.DataAnnotations;
using EventRegistrationAPI_VishvaTyagi.Validation;

namespace EventRegistrationAPI_VishvaTyagi.DTOs;

/// <summary>
/// Input DTO for creating a campus event.
/// </summary>
public class CreateEventDto
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(150, ErrorMessage = "Location cannot be longer than 150 characters.")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Event date is required.")]
    [FutureDate(ErrorMessage = "Event date must be in the future.")]
    public DateTime EventDate { get; set; }

    [Range(1, 500, ErrorMessage = "Capacity must be between 1 and 500.")]
    public int Capacity { get; set; }
}
