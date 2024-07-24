using WebApplication103434929_VT.Areas.Identity.Data;

namespace WebApplication103434929_VT.Models
{
    public class Teacher
    {
        public int id { get; set; }
        public string name { get; set; }
        public int MajorId { get; set; }
        public Major Major { get; set; }
        public WebUser WebUser { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
