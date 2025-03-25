namespace SchoolApplication.Models;

public class Grade
{
    public int Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public DateTime DateGraded { get; set; }
    
    
    // Relationships
    public int ClassId { get; set; }
    public int StudentId { get; set; }
    public int GradedByTeacherId { get; set; }

    public Class? Class { get; set; }
    public User? Student { get; set; }
    public User? GradedByTeacher { get; set; }
}