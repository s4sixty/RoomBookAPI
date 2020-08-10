using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RoomBookAPI.Helpers;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;

namespace RoomBookAPI.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class UsersController: Controller
    {
        // Inject the DBContext into the controller...
        private ApiContext _context;
        // Inject the configuration into the controller
        private IConfiguration _configuration;
        // The variable UserClaims will store data from the authorized JWT
        public ClaimsPrincipal UserClaims { get; }

        public UsersController(ApiContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Return a list of all users
        [HttpGet]
        public async Task<List<User>> GetUsers()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return users;
        }

        // Return a single user from a given id
        [HttpGet("profile")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            int idClaims = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            User user = await _context.Users.FindAsync(idClaims);
            if (user == null)
            {
                return NotFound();
            }
            return user;

        }

        // POST : Create a user
        [HttpPost]
        public async Task<ActionResult<User>> RegisterUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            return BadRequest(new
            {
                error = "Your data is missing or incorrect"
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    error = "Your data is missing or incorrect"
                });
            }

            string jwt;

            User user = await _context.Users.FirstOrDefaultAsync(x => x.Username.Equals(data.Username));
            if (user == null)
            {
                return BadRequest(new
                {
                    error = "User not found"
                });
            }
            else if (user.Password != data.Password)
            {
                return BadRequest(new
                {
                    error = "Password is not valid"
                });
            }
            else
            {
                jwt = CreateToken(user);
            }
            return Ok(new
            {
                token = jwt,
                id = user.Id,
                expires = DateTime.Now.AddHours(1)
            });
        }

        // Function : Create the JWT Token
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            { 
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
