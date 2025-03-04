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
using Capstone.Api.Models.DTOs;

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
            try 
            {
                User user = new User
                {
                    UserName = registration.Email, // Use email as username
                    Email = registration.Email,
                    FirstName = registration.FirstName,
                    LastName = registration.LastName,
                    ImageLocation = registration.ImageLocation,
                    Role = registration.Role,
                    IsVerified = registration.Role == "Photographer" ? false : registration.IsVerified,
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
                        user.ImageLocation,
                        user.IsVerified
                    });
                }

                return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Registration failed", error = ex.Message });
            }
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

        [HttpPost("set-initial-password")]
        [AllowAnonymous]
        public async Task<IActionResult> SetInitialPassword([FromBody] SetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound("User not found");

            // Only allow password change if user is not verified
            if (user.IsVerified)
            {
                return BadRequest("User is already verified and password is set");
            }

            // Remove existing password if any
            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded)
            {
                return BadRequest("Failed to reset existing password");
            }

            // Set the new password
            var result = await _userManager.AddPasswordAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Mark user as verified
            user.IsVerified = true;
            await _userManager.UpdateAsync(user);

            return Ok("Password set successfully");
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return NotFound();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            if (!string.IsNullOrEmpty(model.ImageLocation))
            {
                user.ImageLocation = model.ImageLocation;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

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
    }
} 