using WebApplication103434929_VT.Models;

namespace WebApplication103434929_VT.ModelView
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TestDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RoomLocation { get; set; }
        
        public int RoomId { get; set; }
        
       
    }
}
