using Microsoft.AspNetCore.Identity;
using SchoolApplication.Models;
namespace SchoolApplication.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly AppDBContext _context;
    private readonly UserManager<User> _userManager;

    public AdminController(AppDBContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    // GET TEACHERS LIST
    [HttpGet("teachers")]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachers = await _userManager.Users
            .Where(u => u.Role == Role.Teacher)
            .Include(u => u.TeacherClasses)
            .ThenInclude(tc => tc.Class)
            .Include(u => u.TeacherClasses)
            .ThenInclude(tc => tc.Subject)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.PhoneNumber,
                ClassesTeaching = u.TeacherClasses.Select(tc => new
                {
                    Class = tc.Class.ClassName,
                    Section = tc.Class.SectionName,
                    Subject = tc.Subject.Name
                })
            })
            .ToListAsync();

        return Ok(teachers);
    }



    //============ CREATE A TEACHER (with password hashing)==============
    [HttpPost("teachers")]
    public async Task<ActionResult<User>> CreateTeacher(TeacherCreateDto dto)
    {
         var teacher = new User
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Email = dto.Email,
        UserName = dto.Email,
        Role = Role.Teacher,
        PhoneNumber = dto.PhoneNumber
    };

    var result = await _userManager.CreateAsync(teacher, dto.Password);
    if (!result.Succeeded) return BadRequest(result.Errors);
    
    using var transaction = await _context.Database.BeginTransactionAsync();

    try
    {

    foreach (var assignment in dto.ClassAssignments)
    {
        // 1. Handle Subject
        var subject = await _context.Subjects
            .FirstOrDefaultAsync(s => s.Name == assignment.SubjectName);

        if (subject == null)
        {
            subject = new Subject { Name = assignment.SubjectName };
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync(); // Save Subject FIRST
        }

        // 2. Handle Class (now SubjectId is guaranteed to exist)
        var classEntity = await _context.Classes
            .FirstOrDefaultAsync(c => 
                    c.ClassName == assignment.ClassName &&
                    c.SectionName == assignment.SectionName &&
                    c.SubjectId == subject.Id // Use the saved SubjectId
            );

        if (classEntity == null)
        {
            classEntity = new Class
            {
                ClassName = assignment.ClassName,
                SectionName = assignment.SectionName,
                SubjectId = subject.Id // Use valid SubjectId
            };
            _context.Classes.Add(classEntity);
            await _context.SaveChangesAsync(); // Save Class
        }

        // 3. Handle TeacherClass
        var teacherClass = new TeacherClass
        {
            TeacherId = teacher.Id,
            ClassId = classEntity.Id,
            SubjectId = subject.Id
        };

        if (!await _context.TeacherClasses.AnyAsync(tc => 
                tc.TeacherId == teacher.Id && 
                tc.ClassId == classEntity.Id && 
                tc.SubjectId == subject.Id))
        {
            _context.TeacherClasses.Add(teacherClass);
        }
    }
    await _context.SaveChangesAsync(); // Single save for all changes
    await transaction.CommitAsync(); 
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        return BadRequest($"Error: {ex.Message}");
    }

    await _context.SaveChangesAsync();
    return Ok(teacher);
    }

    // ================= DELETE TEACHER BY ID =================

    [HttpDelete("teachers/{id}")]
    public async Task<IActionResult> DeleteTeacher(int id)
    {
        var teacher = await _userManager.FindByIdAsync(id.ToString());
        if (teacher == null || teacher.Role != Role.Teacher)
            return NotFound("Teacher not found");

        var result = await _userManager.DeleteAsync(teacher);
        return result.Succeeded ? NoContent() : BadRequest(result.Errors);
    }

    
    // ================= UPDATE  TEACHERS =================
    [HttpPut("teachers/{teacherId}")]
    public async Task<ActionResult<TeacherDto>> UpdateTeacher(string teacherId, TeacherCreateDto dto)
    {
        // Validate student exists
        var teacher = await _userManager.FindByIdAsync(teacherId);
        if (teacher == null) return BadRequest(error: "Teacher not found");
    
        // Validate class exists if class name is being updated
      /*  if (!string.IsNullOrEmpty(dto.ClassName))
        {
            var classExists = await _context.Classes.AnyAsync(c => c.ClassName == dto.ClassName);
            if (!classExists) return BadRequest(error: "Invalid Class Name");
        }
        */

        // Update properties
        teacher.FirstName = dto.FirstName ?? teacher.FirstName;
        teacher.LastName = dto.LastName ?? teacher.LastName;
        teacher.PhoneNumber = dto.PhoneNumber ?? teacher.PhoneNumber;
        teacher.Email = dto.Email ?? teacher.Email;
        teacher.UserName = dto.Email ?? teacher.UserName;
       
        
        // Save updates
        var updateResult = await _userManager.UpdateAsync(teacher);
        if (!updateResult.Succeeded) return BadRequest(updateResult.Errors);

        return Ok(new TeacherDto
        {
            TeacherId  = teacher.Id,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            Email = teacher.Email,
            PhoneNumber = teacher.PhoneNumber,
        });
    }
    // ================= ADD CLASSES[ADMIN] =================

  //DIRECT INSERT SCRIPT

    // ================= GET CLASSES[ADMIN] =================
    [HttpGet("classes")]
    public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses()
    {
        return await _context.Classes
            .Include(c => c.Teacher)
            .Select(c => new ClassDto
            {
                Id=c.Id,
                ClassName = c.ClassName,
                SectionName = c.SectionName
                
            })
            .ToListAsync();
    }


// ================= STUDENT ENDPOINTS =================

    // Get all students
    [HttpGet("students")]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
    {
        return await _userManager.Users
            .Where(u => u.Role == Role.Student)
            .Include(u => u.Class)
            .Select(u => new StudentDto
            {
                StudentId = u.Id,
                StudentLastName =u.LastName,
                StudentFirstName = u.FirstName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                //ClassName = u.ClassName,
                //SectionName = u.SectionName
            })
            .ToListAsync();
    }
    // Create a new student
    [HttpPost("students")]
    public async Task<ActionResult<StudentDto>> CreateStudent(StudentCreateDto dto)
    {
        // Validate class exists
        var classExists = await _context.Classes.AnyAsync(c => c.ClassName == dto.ClassName);
        if (!classExists) return BadRequest("Invalid Class ID");
        var student = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            UserName = dto.Email,
            Role = Role.Student,
            //ClassName = dto.ClassName,
            //SectionName = dto.SectionName
        };

        var result = await _userManager.CreateAsync(student, dto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return CreatedAtAction(nameof(GetStudents), new StudentDto
        {
            StudentId = student.Id,
            StudentFirstName = student.FirstName,
            StudentLastName=student.LastName,
            Email = student.Email,
            PhoneNumber = student.PhoneNumber,
            ClassName = "N/A" // Will update after creation
        });
    }
   
    // Update an existing student
    [HttpPut("students/{studentId}")]
    public async Task<ActionResult<StudentDto>> UpdateStudent(string studentId, StudentCreateDto dto)
    {
        // Validate student exists
        var student = await _userManager.FindByIdAsync(studentId);
        if (student == null) return BadRequest(error: "Student not found");
    
        // Validate class exists if class name is being updated
        if (!string.IsNullOrEmpty(dto.ClassName))
        {
            var classExists = await _context.Classes.AnyAsync(c => c.ClassName == dto.ClassName);
            if (!classExists) return BadRequest(error: "Invalid Class Name");
        }

        // Update properties
        student.FirstName = dto.FirstName ?? student.FirstName;
        student.LastName = dto.LastName ?? student.LastName;
        student.PhoneNumber = dto.PhoneNumber ?? student.PhoneNumber;
        student.Email = dto.Email ?? student.Email;
        student.UserName = dto.Email ?? student.UserName;
        //student.ClassName = dto.ClassName ?? student.ClassName;
        //student.SectionName = dto.SectionName ?? student.SectionName;
        
        // Save updates
        var updateResult = await _userManager.UpdateAsync(student);
        if (!updateResult.Succeeded) return BadRequest(updateResult.Errors);

        return Ok(new StudentDto
        {
            StudentId = student.Id,
            StudentFirstName = student.FirstName,
            StudentLastName = student.LastName,
            Email = student.Email,
            PhoneNumber = student.PhoneNumber,
            //ClassName = student.ClassName ?? "N/A",
            //SectionName = student.SectionName ?? "N/A"
        });
    }
    
    // DELETE: api/students/{studentId}
    [HttpDelete("students/{studentId}")]
    public async Task<IActionResult> DeleteStudent(string studentId)
    {
        try
        {
            // Validate student ID
            if (string.IsNullOrWhiteSpace(studentId))
                return BadRequest("Invalid Student ID");

            // Find student
            var student = await _userManager.FindByIdAsync(studentId);
            if (student == null)
                return NotFound("Student not found");

            // Verify student role
            if (student.Role != Role.Student)
                return BadRequest("Only students can be deleted through this endpoint");

            // Delete student
            var result = await _userManager.DeleteAsync(student);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Return success
            return NoContent(); // 204 No Content
        }
        catch (Exception ex)
        {
            // Log exception
          
            return StatusCode(500, "An error occurred while deleting the student");
        }
    }
}