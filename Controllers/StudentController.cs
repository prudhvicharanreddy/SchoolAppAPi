namespace SchoolApplication.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApplication.Models;

//[Authorize(Roles = "Student")]
[ApiController]
[Route("api/student")]
public class StudentController : ControllerBase
{
    
   
        private readonly AppDBContext _context;

        public StudentController(AppDBContext context)
        {
            _context = context;
        }

        // Get syllabus for the student's class
        [HttpGet("syllabus")]
        public async Task<ActionResult<Syllabus>> GetSyllabus()
        {
            var studentEmail = User.Identity?.Name;
            var student = await _context.Users
                .Include(u => u.Class)
                .FirstOrDefaultAsync(u => u.Email == studentEmail);
        
            if (student?.ClassId == null) return NotFound();

            var syllabus = await _context.Syllabi
                .FirstOrDefaultAsync(s => s.ClassId == student.ClassId);
        
            return Ok(syllabus);
        }

        // Get grades for the logged-in student
        [HttpGet("grades/{studentId}")]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGrades(int studentId)
        {
            var grades = await _context.Grades
                .Where(g => g.StudentId == studentId)
                .ToListAsync();

            return Ok(grades);
        }

        // Other endpoints: Get Schedule, Attendance, etc.
    }
