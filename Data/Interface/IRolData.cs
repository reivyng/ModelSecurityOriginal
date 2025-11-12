using Entity.Model;

namespace Data.Interfaces
{
    public interface IRolData : IBaseData<Rol>
    {
        // Obtener rol por id (para autenticación)
        Task<Rol?> GetByIdAsync(int id);
    }
}