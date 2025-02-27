using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Capstone.Api.Models;
using Capstone.Api.Data;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Capstone.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromHeader] string authorization)
        {
            try
            {
                string encodedCredentials = authorization.Substring(6).Trim();
                string credentials = Encoding
                    .GetEncoding("iso-8859-1")
                    .GetString(Convert.FromBase64String(encodedCredentials));

                int separatorIndex = credentials.IndexOf(':');
                string email = credentials.Substring(0, separatorIndex);
                string password = credentials.Substring(separatorIndex + 1);

                User? user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return Unauthorized();
                }

                SignInResult result = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (result.Succeeded)
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName ?? ""),
                        new Claim(ClaimTypes.Email, user.Email ?? ""),
                        new Claim(ClaimTypes.Role, user.Role ?? "")
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return Ok(new 
                    { 
                        user.Id, 
                        user.UserName, 
                        user.Email, 
                        user.Role,
                        user.FirstName,
                        user.LastName,
                        user.ImageLocation
                    });
                }

                return Unauthorized();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Registration registration)
        {
            User user = new User
            {
                UserName = registration.UserName,
                Email = registration.Email,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                ImageLocation = registration.ImageLocation,
                Role = registration.Role,
                CreateDateTime = DateTime.UtcNow
            };

            string password = Encoding
                .GetEncoding("iso-8859-1")
                .GetString(Convert.FromBase64String(registration.Password));

            IdentityResult result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, registration.Role);

                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName ?? ""),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.Role, user.Role ?? "")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Ok(new 
                { 
                    user.Id, 
                    user.UserName, 
                    user.Email, 
                    user.Role,
                    user.FirstName,
                    user.LastName,
                    user.ImageLocation
                });
            }

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound();
            
            User? user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return Ok(new 
                { 
                    user.Id, 
                    user.UserName, 
                    user.Email, 
                    user.Role,
                    user.FirstName,
                    user.LastName,
                    user.ImageLocation,
                    user.IsVerified
                });
            }

            return NotFound();
        }
    }
} 