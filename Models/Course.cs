using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication103434929_VT.Models
{
    public class Course
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public Teacher teacher { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }

        [Required]
        public string DayOfWeek { get; set; } // Day of the week the class is held

        [Required]
        public string Time { get; set; } // Start time of the class
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public DateTime TestDate { get; set; }

        [Required]
        public string RoomLocation { get; set; }
        [Required]
        public int RoomId { get; set; }
        [Required]
        public Room Room { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
