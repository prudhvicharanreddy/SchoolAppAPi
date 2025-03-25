using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Models;

public class ClassCreateDto
{
    
        [Required]
        public string ClassName { get; set; } = string.Empty;

        [Required]
        public string SectionName { get; set; } = string.Empty;
        [Required]
        public string Teacher { get; set; } = string.Empty;

        public int? TeacherId { get; set; } // Optional
    
}