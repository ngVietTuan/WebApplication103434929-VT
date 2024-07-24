namespace WebApplication103434929_VT.ModelView
{
    public class CalendarViewModel
    {
        public DateTime CurrentMonth { get; set; }
        public List<ScheduleEntry> Schedule { get; set; }

        public class ScheduleEntry
        {
            public DateTime EndDate { get; set; }
            public string DayOfWeek { get; set; }
            public string TimePeriod { get; set; }
        }
    }
}
