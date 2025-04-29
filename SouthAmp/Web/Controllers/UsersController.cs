using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SouthAmp.Infrastructure.Identity;
using SouthAmp.Infrastructure.Services;
using SouthAmp.Application.DTOs;
using SouthAmp.Core.Entities;
using SouthAmp.Infrastructure.Data;

namespace SouthAmp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        JwtTokenService jwtTokenService,
        IAuditService auditService,
        IEmailService emailService)
        : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new AppUser { UserName = request.UserName, Email = request.Email };
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            await userManager.AddToRoleAsync(user, request.Role ?? "guest");
            var profile = new AppUserProfile
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash!,
                IsActive = true
            };
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.AppUserProfiles.Add(profile);
                await db.SaveChangesAsync();
            }
            auditService.LogUserAction(user.Id.ToString(), "Register", $"Email: {user.Email}");
            await emailService.SendAsync(user.Email, "Registration Confirmation", "Thank you for registering at SouthAmp!");
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized();
            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return Unauthorized();
            var roles = await userManager.GetRolesAsync(user);
            var token = jwtTokenService.GenerateToken(user, roles);
            return Ok(new { token });
        }
    }
}