using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication103434929_VT.Areas.Identity.Data;
using WebApplication103434929_VT.Data;
using WebApplication103434929_VT.Models;
using WebApplication103434929_VT.ModelView;

namespace WebApplication103434929_VT.Controllers
{
    public class CourseController : Controller
    {
        private readonly WebApplication103434929_VTContext _context;
        private readonly UserManager<WebUser> _userManager;
        public CourseController(WebApplication103434929_VTContext context, UserManager<WebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.TeacherId == 0)
            {
                return Challenge();
            }

            var teacher = await _context.Teachers.Include(s => s.Major).FirstOrDefaultAsync(s => s.id == currentUser.TeacherId);
            if (teacher == null)
            {
                return NotFound();
            }

            int pageSize = 5;
            int skip = (page - 1) * pageSize;

            var classes = await _context.Classes
                .Where(c => c.MajorId == teacher.MajorId && !_context.Courses.Any(course => course.TeacherId == currentUser.TeacherId && course.ClassId == c.Id))
                .Skip(skip)
                .Take(pageSize)
                .Select(c => new ClassViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Credit = c.Credit,
                })
                .ToListAsync();


            var rooms = await _context.Rooms
                .Select(r => new RoomViewModel
                {
                    Id = r.Id,
                    Name = r.BuildingName + " - " + r.Name
                })
                .ToListAsync();

            int totalClasses = await _context.Classes.CountAsync(c => c.MajorId == teacher.MajorId);
            int totalPages = (int)Math.Ceiling(totalClasses / (double)pageSize);

            var model = new PaginatedCourseViewModel
            {
                Classes = classes,
                PageNumber = page,
                TotalPages = totalPages,
                Rooms = rooms // Add rooms to the model
            };

            var teacherCourses = await _context.Courses
                .Where(c => c.TeacherId == currentUser.TeacherId)
                .Select(c => new
                {
                    ClassName = c.Class.Name,
                    MajorName = c.Class.Major.Name,
                    DayOfWeek = c.DayOfWeek,
                    Time = c.Time,
                    RoomName = c.Room.Name,
                    RoomLocation = c.Room.Location
                })
                .ToListAsync();

            ViewBag.TeacherCourses = teacherCourses;

            return View(model);
        }
        
        public async Task<IActionResult> Create([FromBody] CourseAddModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var teacher = await _context.Teachers.FindAsync(currentUser.TeacherId);

                if (teacher == null)
                {
                    return NotFound(new { message = "Teacher not found." });
                }

                Course course = new Course
                {
                    TeacherId = teacher.id,
                    ClassId = model.ClassesId,
                    
                    DayOfWeek = model.DayOfWeek,
                    Time = model.Time,
                    StartDate = model.StartDate,
                    TestDate = model.TestDate,
                    EndDate = model.StartDate.AddMonths(3),
                    RoomId = model.RoomId,
                    RoomLocation = model.RoomLocation
                };

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Course added successfully." });
            }
            return BadRequest(new { message = "Invalid data." });
        }

    }
}
