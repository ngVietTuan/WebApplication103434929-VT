namespace WebApplication103434929_VT.ModelView
{
    public class PaginatedClassViewModel
    {
        public IEnumerable<ClassViewModel> Classes { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
