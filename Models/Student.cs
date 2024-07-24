using System.ComponentModel.DataAnnotations;
using WebApplication103434929_VT.Areas.Identity.Data;

namespace WebApplication103434929_VT.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int MajorId { get; set; }
        public Major Major { get; set; }
        public int Credit {  get; set; }
        public WebUser WebUser { get; set; }

        // One-to-many relationship: A student can enroll in many classes
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
