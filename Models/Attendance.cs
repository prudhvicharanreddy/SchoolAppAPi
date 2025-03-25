namespace SchoolApplication.Models;

public class Attendance
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    
    // Relationships
    public int ClassId { get; set; }
    public Class? Class { get; set; }
    
    public int StudentId { get; set; }
    public User? Student { get; set; }
}