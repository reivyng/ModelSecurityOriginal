
using Entity.Dto;
using Entity.Dto.Menu;
using Entity.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRolFormPermissionBusiness : IBaseBusiness<RolFormPermission, RolFormPermissionDto>
    {
        Task<List<MenuDto>> ObtenerMenu(int userId);
        Task<List<MenuDto>> ObtenerMenuFallback(int userId);
    }
}