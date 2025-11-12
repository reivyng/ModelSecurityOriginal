using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Implements;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Entity.Domain.Config;
using Xunit;

namespace ModelSecurityOriginal.Tests
{
    public class TokenServiceTests
    {
        private readonly JwtSettings _jwtSettings = new JwtSettings
        {
            Key = "ThisIsA32CharLongEncryptionKeyForTests!",
            Issuer = "test",
            Audience = "test",
            AccessTokenExpirationMinutes = 60,
            RefreshTokenExpirationDays = 7
        };

        [Fact]
        public async Task GenerateTokensAsync_ValidCredentials_ReturnsTokens()
        {
            // Arrange
            var user = new User { Id = 1, email = "a@b.com", password = BCrypt.Net.BCrypt.HashPassword("secret"), Rol = new Rol { Id = 5, type_rol = "Admin" } };

            var userDataMock = new Mock<IUserData>();
            userDataMock.Setup(u => u.FindByEmailAsync(user.email)).ReturnsAsync(user);

            var refreshMock = new Mock<IRefreshTokenData>();
            refreshMock.Setup(r => r.AddAsync(It.IsAny<RefreshToken>())).Returns(Task.CompletedTask).Verifiable();

            var tokenService = new TokenService(userDataMock.Object, refreshMock.Object, Options.Create(_jwtSettings), Mock.Of<ILogger<TokenService>>());

            var dto = new Entity.Dto.Auth.LoginUserDto { email = user.email, password = "secret" };

            // Act
            var (access, refresh) = await tokenService.GenerateTokensAsync(dto);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(access));
            Assert.False(string.IsNullOrWhiteSpace(refresh));
            refreshMock.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Once);

            // decode token and assert role claims
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(access);
            // Some JWT serializers map claim types (e.g. ClaimTypes.Role) to short names like "role".
            // Assert by value as a robust fallback.
            var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role || c.Type == "role" || c.Value == "Admin");
            Assert.NotNull(roleClaim);
            Assert.Equal("Admin", roleClaim.Value);
            var rolIdClaim = jwt.Claims.FirstOrDefault(c => c.Type == "rol_id" || c.Value == "5");
            Assert.NotNull(rolIdClaim);
            Assert.Equal("5", rolIdClaim.Value);
        }

        [Fact]
        public async Task GenerateTokensAsync_InvalidPassword_ThrowsUnauthorized()
        {
            var user = new User { Id = 1, email = "a@b.com", password = BCrypt.Net.BCrypt.HashPassword("secret") };
            var userDataMock = new Mock<IUserData>();
            userDataMock.Setup(u => u.FindByEmailAsync(user.email)).ReturnsAsync(user);

            var refreshMock = new Mock<IRefreshTokenData>();

            var tokenService = new TokenService(userDataMock.Object, refreshMock.Object, Options.Create(_jwtSettings), Mock.Of<ILogger<TokenService>>());
            var dto = new Entity.Dto.Auth.LoginUserDto { email = user.email, password = "wrong" };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await tokenService.GenerateTokensAsync(dto));
        }

        [Fact]
        public async Task RefreshAsync_ValidRefresh_RotatesTokens()
        {
            // Arrange
            var plain = "myrefreshplain";
            // compute expected hash using same HMAC-SHA512 logic
            var keyBytes = System.Text.Encoding.UTF8.GetBytes(_jwtSettings.Key);
            using var hmac = new System.Security.Cryptography.HMACSHA512(keyBytes);
            var mac = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(plain));
            var hash = Convert.ToHexString(mac).ToLowerInvariant();

            var record = new RefreshToken { Id = 1, user_id = 2, token_hash = hash, expires_at = DateTime.UtcNow.AddDays(1), is_revoked = false };

            var refreshMock = new Mock<IRefreshTokenData>();
            refreshMock.Setup(r => r.GetByHashAsync(hash)).ReturnsAsync(record);
            refreshMock.Setup(r => r.RevokeAsync(record, It.IsAny<string>())).Returns(Task.CompletedTask).Verifiable();
            refreshMock.Setup(r => r.AddAsync(It.IsAny<RefreshToken>())).Returns(Task.CompletedTask).Verifiable();

            var user = new User { Id = 2, email = "u@u.com", password = BCrypt.Net.BCrypt.HashPassword("x"), Rol = new Rol { Id = 7, type_rol = "User" } };
            var userDataMock = new Mock<IUserData>();
            userDataMock.Setup(u => u.GetByIdWithRoleAsync(2)).ReturnsAsync(user);

            var tokenService = new TokenService(userDataMock.Object, refreshMock.Object, Options.Create(_jwtSettings), Mock.Of<ILogger<TokenService>>());

            // Act
            var (newAccess, newRefresh) = await tokenService.RefreshAsync(plain);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(newAccess));
            Assert.False(string.IsNullOrWhiteSpace(newRefresh));
            refreshMock.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Once);
            refreshMock.Verify(r => r.RevokeAsync(record, It.IsAny<string>()), Times.Once);
            // assert role and rol_id present in newAccess
            var handler2 = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwt2 = handler2.ReadJwtToken(newAccess);
            var roleClaim2 = jwt2.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role || c.Type == "role" || c.Value == "User");
            Assert.NotNull(roleClaim2);
            Assert.Equal("User", roleClaim2.Value);
            var rolIdClaim2 = jwt2.Claims.FirstOrDefault(c => c.Type == "rol_id" || c.Value == "7");
            Assert.NotNull(rolIdClaim2);
            Assert.Equal("7", rolIdClaim2.Value);
        }
    }
}
