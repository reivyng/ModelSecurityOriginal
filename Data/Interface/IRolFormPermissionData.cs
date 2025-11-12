using Entity.Model;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRolFormPermissionData : IBaseData<RolFormPermission>
    {
        // Returns the Rol entity (with navigation properties) for the user's role
        Task<Rol> ObtenerMenu(int userId);
    }
}