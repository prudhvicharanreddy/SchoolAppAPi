namespace SchoolApplication.Models;

public class Schedule
{
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Subject { get; set; } = string.Empty;
    
    // Relationships
    public int ClassId { get; set; }
    public Class? Class { get; set; }
}
public enum DayOfWeek
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}