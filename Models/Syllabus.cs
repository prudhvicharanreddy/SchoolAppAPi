namespace SchoolApplication.Models;

public class Syllabus
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Relationships
    public int ClassId { get; set; }
    public Class? Class { get; set; }
    
    public int CreatedByTeacherId { get; set; }
    public User? CreatedByTeacher { get; set; }
}