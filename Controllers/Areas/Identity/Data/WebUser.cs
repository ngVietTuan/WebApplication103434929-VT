using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApplication103434929_VT.Models;

namespace WebApplication103434929_VT.Areas.Identity.Data;

// Add profile data for application users by adding properties to the WebUser class
public class WebUser : IdentityUser
{
    [Required]
    public string? name { get; set; }
    public string? address { get; set; }
    public int? StudentId { get; set; }
    public Student? student { get; set; }
    public int? TeacherId { get; set; }
    public Teacher? Teacher { get; set;}

    
    
}

