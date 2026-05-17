using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RailwayReservation.DTOs;
using RailwayReservation.Models; 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RailwayReservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // Changed IdentityUser to ApplicationUser to match your Program.cs configuration
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null) 
                return BadRequest(new { message = "Username is already taken!" });

            // Using ApplicationUser here allows us to save model.FullName
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FullName = model.FullName // This works because of ApplicationUser
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded) 
                return BadRequest(new { message = "User creation failed.", errors = result.Errors });

            // Ensure 'Passenger' role exists
            if (!await _roleManager.RoleExistsAsync("Passenger"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Passenger"));
            }
            
            await _userManager.AddToRoleAsync(user, "Passenger");

            return Ok(new { Message = "Registration successful! You can now login as a Passenger." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id), // Identity IDs are strings by default
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new 
                { 
                    token = new JwtSecurityTokenHandler().WriteToken(token), 
                    expiration = token.ValidTo,
                    username = user.UserName,
                    role = userRoles.FirstOrDefault() // Helpful for the frontend to know the role
                });
            }
            return Unauthorized(new { message = "Invalid username or password." });
        }
    }
}