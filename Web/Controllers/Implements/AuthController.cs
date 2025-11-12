using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto.Auth;
using Entity.Model;
using System.Linq;
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
        private readonly IRefreshTokenData _refreshData;
        private readonly ITokenBusiness _tokenService;
        private readonly JwtSettings _jwt;
        private readonly Business.Interfaces.IAuthBusiness _authService;

        public AuthController(IRefreshTokenData refreshData, ITokenBusiness tokenService, IOptions<JwtSettings> jwtOptions, Business.Interfaces.IAuthBusiness authService)
        {
            _refreshData = refreshData;
            _tokenService = tokenService;
            _jwt = jwtOptions.Value;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (dto.password != dto.confirm_password) return BadRequest(new { message = "Passwords do not match" });

            var created = await _authService.RegisterAsync(dto);
            if (!created) return BadRequest(new { message = "Email already registered or registration failed" });

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
