using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bugz.Server.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;
        public AdminController(UserManager<User> userManager, ILoggerManager logger)
        {
            _logger = logger;
            _userManager = userManager;

        }

        [HttpPost("roles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleUpdateDto roleUpdateDto)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return NotFound("User Not Found");

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = roleUpdateDto.Roles;

            selectedRoles = selectedRoles ?? new string[] { };
            var addResult = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!addResult.Succeeded)
                return BadRequest("Failed to add to roles");

            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!removeResult.Succeeded)
                return BadRequest("Failed to remove the roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }
    }
}