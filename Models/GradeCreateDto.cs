namespace SchoolApplication.Models;

public class GradeCreateDto
{
    
        public string Subject { get; set; }
        public int Score { get; set; }
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public int GradedByTeacherId { get; set; }
    
}