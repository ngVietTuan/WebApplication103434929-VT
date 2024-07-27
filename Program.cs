using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApplication103434929_VT.Areas.Identity.Data;
using WebApplication103434929_VT.Data;
using WebApplication103434929_VT.Service;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebApplication103434929_VTContextConnection")
    ?? throw new InvalidOperationException("Connection string 'WebApplication103434929_VTContextConnection' not found.");

builder.Services.AddDbContext<WebApplication103434929_VTContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<WebUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Add roles to the Identity service
    .AddEntityFrameworkStores<WebApplication103434929_VTContext>();

builder.Services.AddBusessServices();
// Add services to the container.

builder.Services.AddScoped<UserManagementService>();
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
});
builder.Services.AddRazorPages(); // Ensure Razor Pages are added

var app = builder.Build();

await SeedData.SeedAdminUser(app.Services, "admin@domain.com", "Admin@123");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages(); // Ensure this is present to map the Razor pages, including Identity

app.Run();

public static class SeedData
{
    public static async Task SeedAdminUser(IServiceProvider serviceProvider, string adminUserName, string adminPassword)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<WebUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure the admin role exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Check if the admin user exists
            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new WebUser
                {
                    UserName = adminUserName,
                    Email = adminUserName,
                    EmailConfirmed = true // Ensure required fields are set
                };

                try
                {
                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        // Handle the creation errors
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine(error.Description);
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"An error occurred while saving the admin user: {ex.InnerException?.Message ?? ex.Message}");
                    throw;
                }
            }
        }
    }
}