using System.ComponentModel.DataAnnotations;

namespace WebApplication103434929_VT.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string BuildingName { get; set; }

        [Required]
        public string Location { get; set; }

        public ICollection<Course> Courses { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
