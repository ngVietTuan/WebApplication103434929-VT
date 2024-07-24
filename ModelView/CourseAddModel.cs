using Microsoft.Build.Framework;

namespace WebApplication103434929_VT.ModelView
{
    public class CourseAddModel
    {

        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int ClassesId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DayOfWeek { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        public int RoomId { get; set; }

        public string RoomLocation { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime TestDate { get; set; }

    }
}
