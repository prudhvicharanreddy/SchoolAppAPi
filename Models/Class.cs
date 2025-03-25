using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApplication.Models;

public class Class
{
    public int Id { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    
    
    
    [ForeignKey("Subject")] 
    public int SubjectId { get; set; } 
    public Subject Subject { get; set; }  // e.g., "Math", "Science", "English"

    
    // Teacher who teaches this class
    public int? TeacherId { get; set; }
    public User? Teacher { get; set; }
    
    // Relationships
    public ICollection<Syllabus>? Syllabi { get; set; }
    public ICollection<Schedule>? Schedules { get; set; }
    public ICollection<Attendance>? Attendances { get; set; }
    public ICollection<Grade>? Grades { get; set; }
    public ICollection<TeacherClass> TeacherClasses { get; set; } = new List<TeacherClass>();
    public ICollection<User> Students { get; set; } = new List<User>();
}