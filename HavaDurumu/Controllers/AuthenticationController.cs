using HavaDurumu.Modeller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HavaDurumu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        public AuthenticationController(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody]ApiUser userData)
        {
            var kullanici = Authentication(userData);
            if(kullanici == null) return NotFound("Kullanıcı Bulunamadı");

            var token = CreateToken(kullanici!);
            return Ok(token);
        }

        private string CreateToken(ApiUser apiUser)
        {
            if (_jwtSettings == null) throw new Exception("Jwt Ayarlarındaki Key Değeri Null Olamaz.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var claimArray = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, apiUser.Name!),
                new Claim(ClaimTypes.Role, apiUser.Role!)
            };

            var token = new JwtSecurityToken(_jwtSettings.Issuer,_jwtSettings.Audience,claimArray,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private ApiUser? Authentication(ApiUser userData)
        {
            return ApiUsers.Users.FirstOrDefault(x =>
                x.Name == userData.Name && 
                x.Password == userData.Password);
        }
    }
}
