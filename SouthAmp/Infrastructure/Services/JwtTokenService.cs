using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SouthAmp.Infrastructure.Identity;

namespace SouthAmp.Infrastructure.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;
        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public virtual string GenerateToken(AppUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // <-- DODANE
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "dev_secret_key"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "SouthAmp",
                audience: _configuration["Jwt:Audience"] ?? "SouthAmpAudience",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds
            );
            Console.WriteLine($"JWT KEY (JwtTokenService): {_configuration["Jwt:Key"]}");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    public static class JwtKeyProvider
    {
        public static string GetJwtKey(IConfiguration configuration, ILogger? logger = null)
        {
            var envKey = Environment.GetEnvironmentVariable("SOUTHAMP_JWT_KEY");
            if (!string.IsNullOrWhiteSpace(envKey))
            {
                logger?.LogInformation("Using JWT key from environment variable SOUTHAMP_JWT_KEY");
                return envKey;
            }
            var configKey = configuration["Jwt:Key"];
            if (!string.IsNullOrWhiteSpace(configKey))
            {
                logger?.LogInformation("Using JWT key from appsettings.json");
                return configKey;
            }
            logger?.LogWarning("Using fallback development JWT key! This is NOT secure for production.");
            return "dev_secret_key_!@#_change_me";
        }
    }
}