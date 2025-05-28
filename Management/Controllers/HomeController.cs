using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Management.Models;
using Microsoft.AspNetCore.Authorization;
using Management.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
                return this.Redirect("~/identity/account/login");

            var tasks = _context.Tasks.ToList();
            return View(tasks);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Account()
        {
            string email = "";
            if (User.Identity.IsAuthenticated)
            {
                email =
                    User.Claims.FirstOrDefault(c => c.Type == "email")?.Value ??
                    User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ??
                    User.Claims.FirstOrDefault(c => c.Type == "Email")?.Value ?? "";
            }

            var model = new AccountViewModel
            {
                Name = User.Identity.Name,
                Email = email
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match or are empty.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                TempData["Message"] = "Password reset successfully. Please log in again.";
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View();
            }
        }

        [HttpGet]
        public IActionResult ResetEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetEmail(string newEmail, string confirmEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail) || newEmail != confirmEmail)
            {
                ModelState.AddModelError("", "Emails do not match or are empty.");
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            // Check if the email is already taken
            var existingUser = await _userManager.FindByEmailAsync(newEmail);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "This email is already in use.");
                return View();
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var result = await _userManager.ChangeEmailAsync(user, newEmail, token);

            if (result.Succeeded)
            {
                TempData["Message"] = "Email changed successfully. Please log in again.";
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View();
            }
        }
    }
}
