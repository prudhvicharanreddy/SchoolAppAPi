using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SchoolApplication.Models;

public class AppDBContext:IdentityDbContext<
    User,        // Your custom user class
    IdentityRole<int>, // Use int for roles
    int,         // Primary key type for users/roles
    IdentityUserClaim<int>, 
    IdentityUserRole<int>, 
    IdentityUserLogin<int>, // Use int for logins
    IdentityRoleClaim<int>, 
    IdentityUserToken<int>>
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }

 
    public DbSet<Class> Classes { get; set; }
    public DbSet<Syllabus> Syllabi { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<TeacherClass> TeacherClasses { get; set; }


    public DbSet<Grade> Grades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure enums to store as strings
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Schedule>()
            .Property(s => s.DayOfWeek)
            .HasConversion<string>();

        // Relationships
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Subject)
            .WithMany(u => u.Classes)
            .HasForeignKey(c => c.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevent cascade delete
        
        modelBuilder.Entity<Class>()
            .HasOne(c => c.Teacher)
            .WithMany(u => u.ClassesTaught)
            .HasForeignKey(c => c.TeacherId)
            .IsRequired(false)  // Make optional
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Class)
            .WithMany(c => c.Students)
            .HasForeignKey(u => u.ClassId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Syllabus>()
            .HasOne(s => s.Class)
            .WithMany(c => c.Syllabi)
            .HasForeignKey(s => s.ClassId);

        modelBuilder.Entity<Attendance>()
            .HasIndex(a => new { a.ClassId, a.StudentId, a.Date })
            .IsUnique();  // Prevent duplicate attendance entries
        
        modelBuilder.Entity<Grade>()
            .HasOne(g => g.GradedByTeacher)  // Fixed typo
            .WithMany(u => u.Grades)
            .HasForeignKey(g => g.GradedByTeacherId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Grade>()
            .Property(g => g.Score)
            .HasPrecision(5, 2); 
        
        

        modelBuilder.Entity<TeacherClass>()
            .HasKey(tc => new { tc.TeacherId, tc.ClassId, tc.SubjectId });

        // Fix for Teacher relationship
        modelBuilder.Entity<TeacherClass>()
            .HasOne(tc => tc.Teacher)
            .WithMany(u => u.TeacherClasses)
            .HasForeignKey(tc => tc.TeacherId) // Ensure this matches User.Id type
            .OnDelete(DeleteBehavior.Cascade); // Or Restrict if needed

        // Class relationship
        modelBuilder.Entity<TeacherClass>()
            .HasOne(tc => tc.Class)
            .WithMany(c => c.TeacherClasses)
            .HasForeignKey(tc => tc.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        // Subject relationship
        modelBuilder.Entity<TeacherClass>()
            .HasOne(tc => tc.Subject)
            .WithMany(s => s.TeacherClasses)
            .HasForeignKey(tc => tc.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("Subjects"); // Explicit table name
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
        });

        
    }
}