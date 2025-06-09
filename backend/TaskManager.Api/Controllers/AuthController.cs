using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Api.Data;
using TaskManager.Api.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TaskManager.Api.Services;
using System.Linq;
using System;

namespace TaskManager.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest request)
        {
            var result = await _authService.SignupAsync(request.Name, request.Email, request.Phone, request.Password);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(new { message = "Signup successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var expiry = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));
            var result = await _authService.LoginAsync(request.Email, request.Password, expiry);
            if (!result.Success)
                return Unauthorized(result.Message);
            var user = result.User;
            var sessionId = result.SessionId;
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("sessionid", sessionId.ToString())
                }),
                Expires = expiry,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            // Set JWT and sessionid as HttpOnly cookies
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiry
            });

            return Ok(new
            {
                user = new { user.Id, user.Name, user.Email, user.Phone }
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var sessionIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sessionid");
            if (sessionIdClaim != null && Guid.TryParse(sessionIdClaim.Value, out var sessionId))
            {
                await _authService.LogoutAsync(sessionId);
            }
            Response.Cookies.Delete("jwt");
            return Ok(new { message = "Logged out" });
        }

        public class SignupRequest
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Password { get; set; }
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
