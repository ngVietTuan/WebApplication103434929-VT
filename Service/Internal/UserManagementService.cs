using Microsoft.AspNetCore.Identity;
using WebApplication103434929_VT.Areas.Identity.Data;
using WebApplication103434929_VT.Data;
using WebApplication103434929_VT.Models;
using WebApplication103434929_VT.ModelView;

public class UserManagementService
{
    private readonly UserManager<WebUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly WebApplication103434929_VTContext _context;
    private readonly ILogger<UserManagementService> _logger;

    public UserManagementService(UserManager<WebUser> userManager, RoleManager<IdentityRole> roleManager, WebApplication103434929_VTContext context, ILogger<UserManagementService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
    }

    public async Task<(bool success, string message, WebUser user)> CreateUserAsync(UserCreationModel userModel)
    {
        if (userModel == null)
        {
            return (false, "Invalid user data.", null);
        }

        try
        {
            var user = new WebUser
            {
                UserName = userModel.UserName,
                name = userModel.UserName,
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
                address = userModel.Address,
                EmailConfirmed = true // Ensure the email is confirmed
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {
                if (userModel.UserType == "teacher")
                {
                    var teacher = new Teacher { name = userModel.UserName, MajorId = userModel.MajorId };
                    _context.Teachers.Add(teacher);
                    await _context.SaveChangesAsync();
                    user.TeacherId = teacher.id;
                }
                else if (userModel.UserType == "student")
                {
                    var student = new Student { Name = userModel.UserName, MajorId = userModel.MajorId, Credit = 0 };
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();
                    user.StudentId = student.Id;
                }

                await _userManager.UpdateAsync(user);
                return (true, "User created successfully.", user);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("User creation failed: {Errors}", errors);
                return (false, errors, null);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the user.");
            return (false, "An error occurred while creating the user.", null);
        }
    }

    public async Task<(bool success, string message, WebUser user)> EditUserAsync(UserCreationModel userModel)
    {
        if (userModel == null || string.IsNullOrEmpty(userModel.Id))
        {
            return (false, "Invalid user data.", null);
        }

        try
        {
            var user = await _userManager.FindByIdAsync(userModel.Id);
            if (user == null)
            {
                return (false, "User not found.", null);
            }

            user.UserName = userModel.UserName;
            user.Email = userModel.Email;
            user.PhoneNumber = userModel.PhoneNumber;
            user.address = userModel.Address;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return (true, "User updated successfully.", user);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("User update failed: {Errors}", errors);
                return (false, errors, null);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the user.");
            return (false, "An error occurred while updating the user.", null);
        }
    }

    public async Task<(bool success, string message)> DeleteUserAsync(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return (false, "User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return (true, "User deleted successfully.");
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("User deletion failed: {Errors}", errors);
                return (false, errors);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the user.");
            return (false, "An error occurred while deleting the user.");
        }
    }

    public async Task<WebUser> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<List<UserViewModel>> GetAllUsersExceptAdminAsync(int pageNumber, int pageSize)
    {
        var adminRole = await _roleManager.FindByNameAsync("Admin");
        var allUsers = _userManager.Users.ToList();

        var nonAdminUsers = allUsers.Where(u => !_userManager.IsInRoleAsync(u, adminRole.Name).Result)
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToList();

        var userViewModels = new List<UserViewModel>();

        foreach (var user in nonAdminUsers)
        {
            var userType = user.TeacherId.HasValue ? "Teacher" : user.StudentId.HasValue ? "Student" : "Unknown";
            var majorId = user.TeacherId.HasValue
                ? _context.Teachers.FirstOrDefault(t => t.id == user.TeacherId)?.MajorId
                : user.StudentId.HasValue
                    ? _context.Students.FirstOrDefault(s => s.Id == user.StudentId)?.MajorId
                    : (int?)null;
            var majorName = majorId.HasValue ? _context.Majors.FirstOrDefault(m => m.Id == majorId)?.Name : "N/A";

            userViewModels.Add(new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                UserType = userType,
                Major = majorName
            });
        }

        return userViewModels;
    }

    public List<Major> GetMajors()
    {
        return _context.Majors.ToList();
    }
}
