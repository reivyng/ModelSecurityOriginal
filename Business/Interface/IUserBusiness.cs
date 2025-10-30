
using Entity.Dto;
using Entity.Model;

namespace Business.Interfaces
{
    public interface IUserBusiness : IBaseBusiness<User, UserDto>
    {
        // Obtener usuarios con filtros específicos de User
        Task<List<UserDto>> GetAllAsync(bool? active, string? emailSearch);
    }
}