using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;

namespace Data.Implements
{
    public class UserData : BaseData.BaseModelData<User>, IUserData
    {
        public UserData(ApplicationDbContext context) : base(context) { }
        // Métodos adicionales específicos de User

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task<User?> GetByIdWithRoleAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}