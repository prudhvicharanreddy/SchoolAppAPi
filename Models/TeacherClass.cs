namespace SchoolApplication.Models;

public class TeacherClass
{
    public int TeacherId { get; set; }
 
    public int ClassId { get; set; }
    public int SubjectId { get; set; }
    public User Teacher { get; set; }
    public Class Class { get; set; }
    public Subject Subject { get; set; }
}