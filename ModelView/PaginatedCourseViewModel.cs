using WebApplication103434929_VT.Models;

namespace WebApplication103434929_VT.ModelView
{
    public class PaginatedCourseViewModel
    {
        public IEnumerable<ClassViewModel> Classes { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public List<RoomViewModel> Rooms { get; set; }
    }
}
