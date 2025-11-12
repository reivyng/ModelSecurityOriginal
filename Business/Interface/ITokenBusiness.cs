using Entity.Model;
using Entity.Dto;
using System.Threading.Tasks;
using Entity.Dto.Auth;

namespace Business.Interfaces
{
    public interface ITokenBusiness
    {
        Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(LoginUserDto dto);
        Task<(string NewAccessToken, string NewRefreshToken)> RefreshAsync(string refreshTokenPlain);
        Task RevokeRefreshTokenAsync(string refreshTokenPlain);
    }
}
