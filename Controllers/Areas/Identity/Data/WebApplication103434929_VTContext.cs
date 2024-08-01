using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication103434929_VT.Areas.Identity.Data;
using WebApplication103434929_VT.Models;

namespace WebApplication103434929_VT.Data
{
    public class WebApplication103434929_VTContext : IdentityDbContext<WebUser>
    {
        public WebApplication103434929_VTContext(DbContextOptions<WebApplication103434929_VTContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<WebUser> WebUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Course>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Course>()
                .HasOne(c => c.teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Class)
                .WithMany(cl => cl.Courses)
                .HasForeignKey(c => c.ClassId);

            modelBuilder.Entity<WebUser>()
                .HasOne(w => w.student)
                .WithOne(s => s.WebUser)
                .HasForeignKey<WebUser>(w => w.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WebUser>()
                .HasOne(w => w.Teacher)
                .WithOne(t => t.WebUser)
                .HasForeignKey<WebUser>(w => w.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}