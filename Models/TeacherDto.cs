namespace SchoolApplication.Models;

public class TeacherDto
{
    public  required int TeacherId { get; set; }
    public  required string FirstName { get; set; }
    public  required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
}