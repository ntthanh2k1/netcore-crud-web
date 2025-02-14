using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.Crud.Web.Data;
using NetCore.Crud.Web.Dtos.AuthDtos;
using NetCore.Crud.Web.Dtos.RoleDtos;
using NetCore.Crud.Web.Models;

namespace NetCore.Crud.Web.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly Context _context;
        private readonly ILogger<AuthController> _logger;
        private readonly RoleManager<Role> _roleManager;
        public RolesController(
            Context context,
            ILogger<AuthController> logger,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _context.Roles
                .Select(a => new GetAllRolesDto
                {
                    Id = a.Id,
                    Code = a.Code,
                    Name = a.Name
                }).ToListAsync();

            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleDto createRoleDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(createRoleDto.Name);

                    if (roleExists)
                    {
                        ViewBag.Message = $"Role \"{createRoleDto.Name}\" already exists.";
                        return View(createRoleDto);
                    }

                    var role = new Role
                    {
                        Code = $"ROLE-{DateTime.Now:yyyyMMdd}",
                        Name = createRoleDto.Name,
                        Description = createRoleDto.Description
                    };
                    var result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetAllRoles", "Roles");
                    }
                }
                
                return View(createRoleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the CreateRole module.");

                // Show a user-friendly error message
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(createRoleDto);
            }
        }


    }
}
