using Microsoft.AspNetCore.Identity;

namespace SchoolApplication.Models;

public class User : IdentityUser<int> // Inherits Id, Email, PasswordHash, etc.
{
    public Role Role { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    // For Students
    public int? ClassId { get; set; }
   
    public Class? Class { get; set; }  // Navigation for Student's class
    
    // For Teachers
    public string? Subject { get; set; }
   
    public ICollection<TeacherClass> TeacherClasses { get; set; } = new List<TeacherClass>();
    public ICollection<Class> ClassesTaught { get; set; } = new List<Class>();// Navigation for Teacher's classe
    public ICollection<Syllabus>? SyllabiCreated { get; set; }
    public ICollection<Attendance>? Attendances { get; set; } 
    public ICollection<Grade>? Grades { get; set; }
}

public enum Role
{
    Admin,
    Teacher,
    Student
}