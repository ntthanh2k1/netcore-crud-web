using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore.Crud.Web.Data;
using NetCore.Crud.Web.Dtos.RoleDtos;
using NetCore.Crud.Web.Models;

namespace NetCore.Crud.Web.Controllers
{
    [Authorize(Roles = "admin")]
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

        #region Get all roles
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _context.Roles
                .Select(a => new GetAllRolesDto
                {
                    Id = a.Id,
                    Code = a.Code,
                    Name = a.Name!
                }).ToListAsync();

            return View(roles);
        }
        #endregion

        #region Get role by id
        [HttpGet]
        public async Task<IActionResult> GetRoleById(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("GetAllRoles", "Roles");
            }

            var roles = await _context.Roles
                .Select(a => new GetRoleByIdDto
                {
                    Id = a.Id,
                    Code = a.Code,
                    Name = a.Name!,
                    Description= a.Description
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            if (roles == null)
            {
                return RedirectToAction("GetAllRoles", "Roles");
            }

            return View(roles);
        }
        #endregion

        #region Create role
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
                        ViewBag.Message = $"Role {createRoleDto.Name} already exists.";
                        return View(createRoleDto);
                    }

                    var role = new Role
                    {
                        Code = $"ROLE-{DateTime.Now:yyyyMMddHHmmss}",
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
        #endregion

        #region Update role
        [HttpGet]
        public async Task <IActionResult> UpdateRole(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("GetAllRoles", "Roles");
            }

            var roles = await _context.Roles
                .Select(a => new UpdateRoleDto
                {
                    Id = a.Id,
                    Code = a.Code,
                    Name = a.Name!,
                    Description = a.Description
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            if (roles == null)
            {
                return RedirectToAction("GetAllRoles", "Roles");
            }

            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(int id, UpdateRoleDto updateRoleDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id != updateRoleDto.Id)
                    {
                        return RedirectToAction("GetAllRoles", "Roles");
                    }

                    var role = await _roleManager.FindByIdAsync(id.ToString());

                    if (role == null)
                    {
                        return View(updateRoleDto);
                    }

                    role.Name = updateRoleDto.Name;
                    role.Description = updateRoleDto.Description;
                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetAllRoles", "Roles");
                    }
                }

                return View(updateRoleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the UpdateRole module.");

                // Show a user-friendly error message
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(updateRoleDto);
            }
        }
        #endregion

        #region Delete role
        [HttpGet]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role == null)
            {
                return RedirectToAction("GetAllRoles", "Roles");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("GetAllRoles", "Roles");
            }

            return RedirectToAction("GetAllRoles", "Roles");
        }
        #endregion
    }
}
