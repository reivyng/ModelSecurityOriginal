using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Implements
{
    public class UserData : BaseData.BaseModelData<User>, IUserData
    {
        public UserData(ApplicationDbContext context) : base(context) { }
        // Métodos adicionales específicos de User

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task<List<User>> GetAllAsync(bool? active, string? emailSearch)
        {
            var query = _dbSet.AsQueryable();

            if (active.HasValue)
            {
                query = query.Where(u => u.active == active.Value);
            }

            if (!string.IsNullOrWhiteSpace(emailSearch))
            {
                var s = emailSearch.Trim().ToLower();
                query = query.Where(u => u.email != null && u.email.ToLower().Contains(s));
            }

            return await query.ToListAsync();
        }
    }
}