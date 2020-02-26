using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
    public class UsersController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        public UsersController(IRepositoryWrapper repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repository.User.GetAllUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("{email}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string email)
        {
            var isCurrentUser = User.FindFirstValue(ClaimTypes.Name) == email;
            var roles = User.FindAll(ClaimTypes.Role);
            bool isAdmin = false;
            
            foreach (var item in roles)
            {
                
                if (item.Value == "Administrator")
                {
                    isAdmin = true;
                    break;
                } 
            }
            
            if (!isCurrentUser && !isAdmin)
                return BadRequest("You are not Authorized");
            
            var user = await _repository.User.GetUser(email);

            if (user == null)
                return NotFound();

            var userToReturn = _mapper.Map<UserForListDto>(user);

            return Ok(userToReturn);
        }
    }
}