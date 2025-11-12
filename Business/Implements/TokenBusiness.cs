using Business.Interfaces;
using Data.Interfaces;
using Data.Interfaces;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Config;
using Entity.Dto.Auth;

namespace Business.Implements
{
    public class TokenService : ITokenBusiness
    {
        private readonly IUserData _userData;
        private readonly IRefreshTokenData _refreshData;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IUserData userData, IRefreshTokenData refreshData, IOptions<JwtSettings> jwtOptions, ILogger<TokenService> logger)
        {
            _userData = userData;
            _refreshData = refreshData;
            _jwtSettings = jwtOptions.Value;
            _logger = logger;
        }

        public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(LoginUserDto dto)
        {
            // user lookup
            var user = await _userData.FindByEmailAsync(dto.email);
            if (user == null) throw new UnauthorizedAccessException("Credenciales inválidas");

            // verify password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(dto.password, user.password))
                throw new UnauthorizedAccessException("Credenciales inválidas");

            // Include role name and role id if available
            var roles = new List<string>();
            List<Claim>? extraClaims = null;
            if (user.Rol != null)
            {
                if (!string.IsNullOrWhiteSpace(user.Rol.type_rol)) roles.Add(user.Rol.type_rol);
                extraClaims = new List<Claim> { new Claim("rol_id", user.Rol.Id.ToString()) };
            }

            var access = BuildAccessToken(user, roles, extraClaims);

            var refreshPlain = GenerateSecureRandomUrlToken(64);
            var refreshHash = HashRefreshToken(refreshPlain);

            var now = DateTime.UtcNow;
            var refreshEntity = new RefreshToken
            {
                user_id = user.Id,
                token_hash = refreshHash,
                created_at = now,
                expires_at = now.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };

            await _refreshData.AddAsync(refreshEntity);

            return (access, refreshPlain);
        }

        public async Task<(string NewAccessToken, string NewRefreshToken)> RefreshAsync(string refreshTokenPlain)
        {
            var hash = HashRefreshToken(refreshTokenPlain);
            var record = await _refreshData.GetByHashAsync(hash) ?? throw new SecurityTokenException("Refresh token inválido");

            if (record.expires_at <= DateTime.UtcNow) throw new SecurityTokenException("Refresh token expirado");
            if (record.is_revoked)
            {
                var validTokens = await _refreshData.GetValidTokensByUserAsync(record.user_id);
                foreach (var t in validTokens) await _refreshData.RevokeAsync(t);
                throw new SecurityTokenException("Refresh token inválido o reutilizado");
            }

            var user = await _userData.GetByIdWithRoleAsync(record.user_id) ?? throw new SecurityTokenException("Usuario no encontrado");

            // Include role name and role id if available
            var rolesList = new List<string>();
            List<Claim>? extraClaims2 = null;
            if (user.Rol != null)
            {
                if (!string.IsNullOrWhiteSpace(user.Rol.type_rol)) rolesList.Add(user.Rol.type_rol);
                extraClaims2 = new List<Claim> { new Claim("rol_id", user.Rol.Id.ToString()) };
            }

            var newAccess = BuildAccessToken(user, rolesList, extraClaims2);
            var newRefreshPlain = GenerateSecureRandomUrlToken(64);
            var newRefreshHash = HashRefreshToken(newRefreshPlain);

            var now = DateTime.UtcNow;
            var newRefreshEntity = new RefreshToken
            {
                user_id = user.Id,
                token_hash = newRefreshHash,
                created_at = now,
                expires_at = now.AddDays(_jwtSettings.RefreshTokenExpirationDays)
            };

            await _refreshData.AddAsync(newRefreshEntity);
            await _refreshData.RevokeAsync(record, replacedByTokenHash: newRefreshHash);

            return (newAccess, newRefreshPlain);
        }

        public async Task RevokeRefreshTokenAsync(string refreshTokenPlain)
        {
            var hash = HashRefreshToken(refreshTokenPlain);
            var record = await _refreshData.GetByHashAsync(hash);
            if (record != null && !record.is_revoked) await _refreshData.RevokeAsync(record);
        }

        private string BuildAccessToken(User user, IEnumerable<string> roles, IEnumerable<Claim>? extraClaims = null)
        {
            var now = DateTime.UtcNow;
            var accessExp = now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            foreach (var r in roles.Where(r => !string.IsNullOrWhiteSpace(r)).Distinct())
            {
                // Add both the full ClaimTypes.Role and the short "role" claim to maximize compatibility
                claims.Add(new Claim(ClaimTypes.Role, r));
                // avoid adding duplicate short 'role' claims
                if (!claims.Any(c => c.Type == "role" && c.Value == r))
                    claims.Add(new Claim("role", r));
            }

            if (extraClaims != null)
            {
                foreach (var c in extraClaims)
                {
                    // avoid duplicate claim types
                    if (!claims.Any(x => x.Type == c.Type && x.Value == c.Value))
                        claims.Add(c);
                }
            }

            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: now,
                expires: accessExp,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private string HashRefreshToken(string token)
        {
            var pepper = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            using var hmac = new HMACSHA512(pepper);
            var bytes = Encoding.UTF8.GetBytes(token);
            var mac = hmac.ComputeHash(bytes);
            return Convert.ToHexString(mac).ToLowerInvariant();
        }

        private static string GenerateSecureRandomUrlToken(int length)
        {
            var bytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes).TrimEnd('=');
        }
    }
}
