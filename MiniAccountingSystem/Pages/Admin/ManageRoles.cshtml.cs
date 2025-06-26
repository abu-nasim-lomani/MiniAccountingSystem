using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MiniAccountingSystem.Pages.Admin
{
    public class ManageRolesModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRolesModel(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public string UserEmail { get; set; }

        // This [BindProperty] will hold the list of roles that the user
        // selects on the form.
        [BindProperty]
        public List<string> SelectedRoles { get; set; } = new List<string>();

        // This property will hold all available roles to display on the page.
        public List<IdentityRole> AllRoles { get; set; }

        // This runs when the page is first loaded.
        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            UserEmail = user.Email;

            // Get all roles from the database.
            AllRoles = await _roleManager.Roles.ToListAsync();

            // Get the roles that are currently assigned to this user.
            var userRoles = await _userManager.GetRolesAsync(user);
            SelectedRoles = userRoles.ToList();

            return Page();
        }

        // This runs when the Admin clicks the "Save Roles" button.
        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Get the user's current roles.
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove the user from all their current roles.
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Add the user to the roles that were selected in the form.
            if (SelectedRoles.Any())
            {
                await _userManager.AddToRolesAsync(user, SelectedRoles);
            }

            return RedirectToPage("./Users");
        }
    }
}