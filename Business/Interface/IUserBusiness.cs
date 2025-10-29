
using Entity.Dto;
using Entity.Model;

namespace Business.Interfaces
{
    public interface IUserBusiness : IBaseBusiness<User, UserDto>
    {
        // Métodos adicionales específicos de User si se requieren
    }
}