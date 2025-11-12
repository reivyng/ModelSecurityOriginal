using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Implements
{
    public class RefreshTokenData : IRefreshTokenData
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenData(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetByHashAsync(string hash)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(r => r.token_hash == hash);
        }

        public async Task<List<RefreshToken>> GetValidTokensByUserAsync(int userId)
        {
            var now = System.DateTime.UtcNow;
            return await _context.RefreshTokens
                .Where(r => r.user_id == userId && !r.is_revoked && r.expires_at > now)
                .ToListAsync();
        }

        public async Task RevokeAsync(RefreshToken token, string? replacedByTokenHash = null)
        {
            token.is_revoked = true;
            token.replaced_by_token_hash = replacedByTokenHash;
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }
    }
}
