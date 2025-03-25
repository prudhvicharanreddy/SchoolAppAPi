namespace SchoolApplication.Models;


    public class TeacherCreateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // Input only
        
        public  string PhoneNumber { get; set; } = string.Empty;
        public List<ClassAssignmentDto> ClassAssignments { get; set; } = new List<ClassAssignmentDto>();    }
