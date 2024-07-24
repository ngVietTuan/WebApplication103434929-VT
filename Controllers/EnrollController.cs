using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication103434929_VT.Areas.Identity.Data;
using WebApplication103434929_VT.Data;
using WebApplication103434929_VT.Models;
using WebApplication103434929_VT.ModelView;

namespace WebApplication103434929_VT.Controllers
{
    [Route("[controller]")]
    public class EnrollController : Controller
    {
        private readonly WebApplication103434929_VTContext _context;
        private readonly UserManager<WebUser> _userManager;

        public EnrollController(WebApplication103434929_VTContext context, UserManager<WebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.StudentId == 0)
            {
                return Challenge();
            }

            var student = await _context.Students.Include(s => s.Major).FirstOrDefaultAsync(s => s.Id == currentUser.StudentId);
            if (student == null)
            {
                return NotFound();
            }

            // Get the list of course IDs the student is already enrolled in
            var enrolledCourseIds = await _context.Enrollments
                .Where(e => e.StudentId == student.Id)
                .Select(e => e.CourseId)
                .ToListAsync();

            int pageSize = 5;
            int skip = (page - 1) * pageSize;

            var classes = await _context.Courses
                .Where(c => c.Class.MajorId == student.MajorId && !enrolledCourseIds.Contains(c.Id))
                .Skip(skip)
                .Take(pageSize)
                .Select(c => new ClassViewModel
                {
                    Id = c.Id,
                    Name = c.Class.Name,
                    Credit = c.Class.Credit,
                })
                .ToListAsync();

            int totalClasses = await _context.Courses
                .CountAsync(c => c.Class.MajorId == student.MajorId && !enrolledCourseIds.Contains(c.Id));

            int totalPages = (int)Math.Ceiling(totalClasses / (double)pageSize);

            var model = new PaginatedClassViewModel
            {
                Classes = classes,
                PageNumber = page,
                TotalPages = totalPages
            };

            // Get the enrolled classes for the ViewBag
            var enrolledClasses = await _context.Courses
                .Where(c => enrolledCourseIds.Contains(c.Id))
                .Select(c => new
                {
                    MajorName = c.Class.Major.Name,
                    ClassName = c.Class.Name,
                    c.DayOfWeek,
                    c.Time,
                    RoomName = c.Room.Name,
                    RoomLocation = c.Room.Location
                })
                .ToListAsync();

            ViewBag.EnrolledClasses = enrolledClasses;

            return View(model);
        }


        [HttpPost("ConfirmEnrollment")]
        public async Task<IActionResult> ConfirmEnrollment([FromBody] List<int> selectedClasses)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null || currentUser.StudentId == 0)
                {
                    return Challenge();
                }

                var student = await _context.Students.FindAsync(currentUser.StudentId);
                if (student == null)
                {
                    return NotFound();
                }

                foreach (var classToEnroll in selectedClasses)
                {
                    // Use raw SQL to add enrollment
                    var sql = "INSERT INTO Enrollments (StudentId, CourseId) VALUES (@StudentId, @CourseId)";
                    await _context.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@StudentId", student.Id), new SqlParameter("@CourseId", classToEnroll));
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the exception (you can replace Console.WriteLine with your logging mechanism)
                Console.WriteLine($"An error occurred while enrolling: {ex.Message}");

                // Optionally, add more details to the log
                Console.WriteLine(ex.StackTrace);

                // Return a user-friendly error message
                return StatusCode(500, new { message = "An error occurred while enrolling. Please try again later." });
            }
        }
    }
}
