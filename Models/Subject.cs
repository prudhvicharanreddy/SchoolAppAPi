namespace SchoolApplication.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Subject
{

        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public List<Class> Classes { get; set; } = new List<Class>();
        public List<TeacherClass> TeacherClasses { get; set; } = new List<TeacherClass>();
    
}