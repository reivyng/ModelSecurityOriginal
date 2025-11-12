using Entity.Model;

namespace Data.Interfaces
{
    public interface IUserData : IBaseData<User>
    {
        // Métodos adicionales específicos de User si se requieren
        Task<User?> FindByEmailAsync(string email);
        Task<User?> GetByIdWithRoleAsync(int id);
    }
}