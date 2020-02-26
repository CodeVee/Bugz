using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Bugz.Server.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager,

        ILoggerManager logger, IMapper mapper, IConfiguration configuration)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userToCreate = _mapper.Map<User>(userForRegisterDto);
            userToCreate.UserName = userForRegisterDto.Email;

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            if (!result.Succeeded) 
                return BadRequest(result.Errors);

            var rolesResult = await _userManager.AddToRolesAsync(userToCreate, new [] {"Submitter"});

            if (!rolesResult.Succeeded) 
                return BadRequest(rolesResult.Errors);

            var userToReturn = _mapper.Map<UserForDetailedDto>(userToCreate);

            return CreatedAtRoute("GetUser",
                    new { controller = "Users", id = userToCreate.Id }, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var result = await _signInManager.PasswordSignInAsync(userForLoginDto.Email, userForLoginDto.Password, 
                                                                    isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded) 
                return Unauthorized("Invalid Email or Password");

            var appUser = await _userManager.Users
                .FirstOrDefaultAsync(user => user.Email == userForLoginDto.Email);

            var userToReturn = _mapper.Map<UserForListDto>(appUser);

            return Ok(new
            {
                token = GenerateJwtToken(appUser).Result,
                user = userToReturn
            });
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secret = _configuration.GetSection("AppSettings:Secret").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var issuer = _configuration.GetSection("AppSettings:Issuer").Value;
            var audience = _configuration.GetSection("Appsettings:Audience").Value;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = credentials,
                Issuer = issuer,
                Audience = audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}