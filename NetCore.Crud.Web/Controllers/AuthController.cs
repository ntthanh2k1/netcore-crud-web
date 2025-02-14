using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCore.Crud.Web.Dtos.AuthDtos;
using NetCore.Crud.Web.Models;

namespace NetCore.Crud.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthController(
            ILogger<AuthController> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userExists = await _userManager.FindByNameAsync(registerDto.Username);

                    if (userExists != null)
                    {
                        ViewBag.Message = $"User {registerDto.Username} already exists.";
                        return View(registerDto);
                    }

                    var user = new User
                    {
                        Code = $"USER-{DateTime.Now:yyyyMMddHHmmss}",
                        Name = registerDto.Name,
                        UserName = registerDto.Username
                    };
                    var result = await _userManager.CreateAsync(user, registerDto.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login", "Auth");
                    }
                }

                return View(registerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Register module.");

                // Show a user-friendly error message
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(registerDto);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userExists = await _userManager.FindByNameAsync(loginDto.Username);

                    if (userExists == null)
                    {
                        ViewBag.Message = $"User {loginDto.Username} does not exist.";
                        return View(loginDto);
                    }

                    var user = await _signInManager.PasswordSignInAsync(loginDto.Username, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: false);

                    if (user.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                return View(loginDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the Login module.");

                // Show a user-friendly error message
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(loginDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
