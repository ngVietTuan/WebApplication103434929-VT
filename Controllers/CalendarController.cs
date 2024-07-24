using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication103434929_VT.Areas.Identity.Data;
using WebApplication103434929_VT.Data;
using WebApplication103434929_VT.ModelView;
using static WebApplication103434929_VT.ModelView.CalendarViewModel;

namespace WebApplication103434929_VT.Controllers
{
    public class CalendarController(WebApplication103434929_VTContext context, UserManager<WebUser> userManager) : Controller
    {
        private readonly WebApplication103434929_VTContext _context = context;
        private readonly UserManager<WebUser> _userManager = userManager;
        public IActionResult Index()
        {
            var currentMonth = DateTime.Now;

            var user = _userManager.GetUserAsync(User).Result;
            var schedule = new List<ScheduleEntry>();
            // Retrieve the student's schedule from the database
            if (user.TeacherId != null)
            {
                schedule = _context.Courses
                   .Where(e => e.TeacherId == user.TeacherId)
                   .Select(e => new CalendarViewModel.ScheduleEntry
                   {
                       DayOfWeek = e.DayOfWeek,
                       TimePeriod = e.Time,
                       EndDate = e.EndDate

                   })
                   .ToList();
            }
            else {
                schedule = _context.Enrollments
                   .Where(e => e.StudentId == user.StudentId)
                   .Select(e => new CalendarViewModel.ScheduleEntry
                   {
                       DayOfWeek = e.Course.DayOfWeek,
                       TimePeriod = e.Course.Time,
                       EndDate = e.Course.EndDate

                   })
                   .ToList();
            }
            var viewModel = new CalendarViewModel
            {
                CurrentMonth = currentMonth,
                Schedule = schedule
            };

            return View(viewModel);
            

        }
    }
}
