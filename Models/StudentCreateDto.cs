using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Models;

public class StudentCreateDto
{

    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required][EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
    
    [Required] public string PhoneNumber { get; set; } = string.Empty;
    [Required] public string ClassName { get; set; }= string.Empty;
    
    [Required] public string SectionName { get; set; }= string.Empty;

}