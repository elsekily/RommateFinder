using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RoommateFinderAPI.Entities.Models;
using RoommateFinderAPI.Entities.Resources;

namespace RoommateFinderAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] SaveUserResource userAccount)
        {
            var userFromDb = userManager.FindByEmailAsync(userAccount.Email).Result;
            var result = signInManager.CheckPasswordSignInAsync(userFromDb, userAccount.Password, false).Result;


            if (result.Succeeded)
            {
                var tokenString = GenerateJSONWebToken(userFromDb);
                return Ok(new
                {
                    userFromDb.Id,
                    userFromDb.Email,
                    userFromDb.UserName,
                    Roles = userManager.GetRolesAsync(userFromDb).Result,
                    tokenString
                });
            }

            return Unauthorized();
        }
        private string GenerateJSONWebToken(User user)
        {
            var claims = new List<Claim>();
            var roles = userManager.GetRolesAsync(user).Result;
            claims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(120),
                SigningCredentials = credentials
            };
            return tokenHandler.WriteToken(tokenHandler.CreateToken(token));
        }
    }
}