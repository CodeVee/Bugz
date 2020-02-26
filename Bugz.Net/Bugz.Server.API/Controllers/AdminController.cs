using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bugz.Server.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, ILoggerManager logger)
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManager;

        }

        [HttpPost("roles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleUpdateDto roleUpdateDto)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return NotFound("User Not Found");

            var selectedRoles = roleUpdateDto.Roles ?? new string[] { };

            foreach (var role in selectedRoles)
            {
                var roleExist = await _roleManager.RoleExistsAsync(role);
                if (!roleExist)
                    return BadRequest($"Role {role} not available");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
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