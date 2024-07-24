using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication103434929_VT.ModelView;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManagementService _userManagementService;
    private const int PageSize = 10;

    public AdminController(UserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        var allUsers = await _userManagementService.GetAllUsersExceptAdminAsync(pageNumber, PageSize);
        var totalUsers = allUsers.Count;  // Count the total number of non-admin users
        var totalPages = (int)Math.Ceiling(totalUsers / (double)PageSize);

        ViewBag.Majors = _userManagementService.GetMajors();
        ViewBag.CurrentPage = pageNumber;
        ViewBag.TotalPages = totalPages;

        return View(allUsers);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreationModel userModel)
    {
        var (success, message, user) = await _userManagementService.CreateUserAsync(userModel);
        if (success)
        {
            return Json(new { success = true, user });
        }
        else
        {
            return Json(new { success = false, message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> EditUser([FromBody] UserCreationModel userModel)
    {
        var (success, message, user) = await _userManagementService.EditUserAsync(userModel);
        if (success)
        {
            return Json(new { success = true, user });
        }
        else
        {
            return Json(new { success = false, message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var (success, message) = await _userManagementService.DeleteUserAsync(id);
        if (success)
        {
            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false, message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userManagementService.GetUserByIdAsync(id);
        return Json(user);
    }
}