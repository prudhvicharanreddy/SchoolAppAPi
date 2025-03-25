using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // Add this for ClaimTypes
using Microsoft.AspNetCore.Authorization;
using SchoolApplication.Models;

namespace SchoolApplication.Controllers ;

[ApiController]
[Route("api/teacher")]
public class TeacherController(AppDBContext context ) : ControllerBase
{
    private readonly AppDBContext _context=context;
    

    // Get current teacher's class ID (corrected method)
    private async Task<int?> GetCurrentTeacherClassIdAsync()
    {
        var teacherEmail = User.Identity?.Name; // From JWT claims
        var teacher = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == teacherEmail); // Fixed lambda syntax
        return teacher?.ClassId; // Null-conditional operator
    }

    // Create syllabus for the teacher's class (corrected)
    [HttpPost("syllabus")]
    public async Task<ActionResult<Syllabus>> CreateSyllabus(Syllabus syllabus)
    {
        var classId = await GetCurrentTeacherClassIdAsync();
        if (classId == null) return Forbid();

        syllabus.ClassId = classId.Value;
        syllabus.CreatedByTeacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); // Fixed ClaimTypes typo

        _context.Syllabi.Add(syllabus);
        await _context.SaveChangesAsync();

        return Ok(syllabus);
    }

    // Grade a student (corrected)
    [HttpPost("grades")]
    public async Task<ActionResult<Grade>> AddGrade([FromBody] GradeCreateDto gradeDto)
    {
        // Get authenticated teacher's ID
      //  var teacherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
      /*  if (string.IsNullOrEmpty(teacherIdClaim))
        {
            return Unauthorized("User ID claim is missing");
        }

        if (!int.TryParse(teacherIdClaim, out var teacherId))
        {
            return Unauthorized("Invalid user ID format");
        }
        // Validate class access - implement your own validation logic
        var teacherClass = await _context.Classes
            .FirstOrDefaultAsync(tc => tc.Id == gradeDto.ClassId && tc.TeacherId == teacherId);

        if (teacherClass == null)
        {
            return Forbid(); // Teacher doesn't have access to this class
        }
*/
        var grade = new Grade
        {
            Subject = gradeDto.Subject,
            Score = gradeDto.Score,
            ClassId = gradeDto.ClassId,
            StudentId = gradeDto.StudentId,
            GradedByTeacherId = gradeDto.GradedByTeacherId, // replace with teacherId after implementing authentication
            DateGraded = DateTime.UtcNow
        };

        _context.Grades.Add(grade);
        await _context.SaveChangesAsync();

        return Ok(grade);
    }
} // Added closing brace for the class