using System.Threading.Tasks;
using Entity.Dto.Auth;

namespace Business.Interfaces
{
    public interface IAuthBusiness
    {
        Task<bool> RegisterAsync(RegisterUserDto dto);
    }
}
