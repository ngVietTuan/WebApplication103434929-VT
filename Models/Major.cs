using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace WebApplication103434929_VT.Models
{
    public class Major
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Student> Students { get; set; }
        public ICollection<Class> Classes { get; set; }
    }
}
