using System.Collections.Generic;
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
        private Dictionary<string, IList<string>> GetRoles = new Dictionary<string, IList<string>>()
        {
            [Policies.Moderator] = new List<string>() { Policies.Moderator, Policies.Owner, Policies.Roommate },
            [Policies.Owner] = new List<string>() { Policies.Owner },
            [Policies.Roommate] = new List<string>() { Policies.Roommate },
        };

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

        [Authorize(Policy = Policies.Admin)]
        [HttpPost("create/user/moderator")]
        public IActionResult RegisterModerator([FromBody] SaveUserResource userAccount)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            return SaveUserAndAddRoles(userAccount, Policies.Moderator);
        }
        [AllowAnonymous]
        [HttpPost("create/user/{inputRole}")]
        public IActionResult RegisterUser([FromBody] SaveUserResource userAccount, string inputRole)
        {
            if (!ModelState.IsValid || (inputRole != Policies.Owner && inputRole != Policies.Roommate))
                return BadRequest();

            return SaveUserAndAddRoles(userAccount, inputRole);
        }
        private IActionResult SaveUserAndAddRoles(SaveUserResource userAccount, string inputRole)
        {
            var user = new User()
            {
                Email = userAccount.Email,
                UserName = userAccount.UserName
            };
            userManager.CreateAsync(user, userAccount.Password).Wait();
            var registeredUser = userManager.FindByNameAsync(user.UserName).Result;

            if (registeredUser != null)
            {
                foreach (var role in GetRoles[inputRole])
                    userManager.AddToRoleAsync(registeredUser, role).Wait();

                return Ok(new
                {
                    registeredUser.Id,
                    registeredUser.Email,
                    registeredUser.UserName,
                    Roles = userManager.GetRolesAsync(registeredUser).Result,
                });
            }
            return NotFound();
        }
    }
}