// Name: Vishva Tyagi
// StudentId: 991811327

using System.ComponentModel.DataAnnotations;

namespace EventRegistrationAPI_VishvaTyagi.DTOs;

/// <summary>
/// Input DTO for registering a student for an event.
/// RegisteredOn is set by the service, not by the client.
/// </summary>
public class CreateRegistrationDto
{
    [Required(ErrorMessage = "Student name is required.")]
    [StringLength(100, ErrorMessage = "Student name cannot be longer than 100 characters.")]
    public string StudentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
    [StringLength(150, ErrorMessage = "Email cannot be longer than 150 characters.")]
    public string Email { get; set; } = string.Empty;
}
