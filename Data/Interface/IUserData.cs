using Entity.Model;

namespace Data.Interfaces
{
    public interface IUserData : IBaseData<User>
    {
        // Obtener usuarios con filtros específicos de User
        Task<List<User>> GetAllAsync(bool? active, string? emailSearch);
    }
}