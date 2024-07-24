using System.ComponentModel.DataAnnotations;

namespace WebApplication103434929_VT.Models
{
    public class Class
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int MajorId { get; set; }
        public Major Major { get; set; }
        public int Credit { get; set; }
        // One-to-many relationship: A class can have many students enrolled
        public ICollection<Course> Courses { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
