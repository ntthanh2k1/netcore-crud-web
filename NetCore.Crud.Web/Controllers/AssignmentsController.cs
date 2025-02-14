using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetCore.Crud.Web.Data;
using NetCore.Crud.Web.Dtos.AssignmentDtos;
using NetCore.Crud.Web.Dtos.AuthDtos;
using NetCore.Crud.Web.Models;

namespace NetCore.Crud.Web.Controllers
{
    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly Context _context;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<User> _userManager;
        public AssignmentsController(
            Context context,
            ILogger<AuthController> logger,
            UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        #region Get all assignments
        [HttpGet]
        public async Task<IActionResult> GetAllAssignments()
        {
            var assignments = await _context.Assignments
                .Select(a => new GetAllAssignmentsDto
                {
                    Id = a.Id,
                    Code = a.Code,
                    Name = a.Name
                })
                .ToListAsync();

            return View(assignments);
        }
        #endregion
    }
}
