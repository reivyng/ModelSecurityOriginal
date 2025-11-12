using Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRefreshTokenData
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> GetByHashAsync(string hash);
        Task<List<RefreshToken>> GetValidTokensByUserAsync(int userId);
        Task RevokeAsync(RefreshToken token, string? replacedByTokenHash = null);
    }
}
