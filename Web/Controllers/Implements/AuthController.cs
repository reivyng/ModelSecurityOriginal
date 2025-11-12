using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto.Auth;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Entity.Domain.Config;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserData _userData;
        private readonly IRefreshTokenData _refreshData;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwt;

        public AuthController(IUserData userData, IRefreshTokenData refreshData, ITokenService tokenService, IOptions<JwtSettings> jwtOptions)
        {
            _userData = userData;
            _refreshData = refreshData;
            _tokenService = tokenService;
            _jwt = jwtOptions.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (dto.password != dto.confirm_password) return BadRequest(new { message = "Passwords do not match" });

            var userExists = await _userData.FindByEmailAsync(dto.email);
            if (userExists != null) return BadRequest(new { message = "Email already registered" });

            // Create person and user
            var person = new Person
            {
                first_name = dto.first_name,
                first_last_name = dto.first_last_name,
                phone_number = dto.phone_number,
                number_identification = dto.number_identification
            };

            var user = new User
            {
                email = dto.email,
                password = BCrypt.Net.BCrypt.HashPassword(dto.password),
                Person = person,
                active = true
            };

            await _userData.CreateAsync(user);

            return Ok(new { isSuccess = true });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            try
            {
                var (access, refresh) = await _tokenService.GenerateTokensAsync(dto);
                return Ok(new { access_token = access, refresh_token = refresh });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            try
            {
                var (newAccess, newRefresh) = await _tokenService.RefreshAsync(refreshToken);
                return Ok(new { access_token = newAccess, refresh_token = newRefresh });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] string refreshToken)
        {
            await _tokenService.RevokeRefreshTokenAsync(refreshToken);
            return Ok(new { message = "revoked" });
        }
    }
}
