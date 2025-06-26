using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore; // Required for ToListAsync()

namespace MiniAccountingSystem.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public List<UserViewModel> UserList { get; set; }

        public UsersModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // This method runs when the page is loaded.
        public async Task OnGetAsync()
        {
            UserList = new List<UserViewModel>();

            // 1. Get all users from the database.
            var users = await _userManager.Users.ToListAsync();

            // 2. Loop through each user to get their roles.
            foreach (var user in users)
            {
                // 3. Get the list of roles for the current user.
                var roles = await _userManager.GetRolesAsync(user);

                // 4. Create a new UserViewModel and add it to our list.
                UserList.Add(new UserViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    // We join the roles into a single comma-separated string for display.
                    Roles = string.Join(", ", roles)
                });
            }
        }
    }

    // This is a simple ViewModel class to hold the combined data for our page.
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string? Email { get; set; }
        public string Roles { get; set; }
    }
}