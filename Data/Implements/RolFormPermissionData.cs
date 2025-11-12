using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Model;

namespace Data.Implements
{
    public class RolFormPermissionData : BaseData.BaseModelData<RolFormPermission>, IRolFormPermissionData
    {
        public RolFormPermissionData(ApplicationDbContext context) : base(context) { }

        public async Task<Rol> ObtenerMenu(int userId)
        {
            // Query the role that belongs to the given user and eager-load required navigation properties.
            // Use AsSplitQuery to avoid cartesian explosion when including multiple collections.
            var rol = await _context.Roles
                .AsSplitQuery()
                .Include(r => r.RolFormPermission)
                    .ThenInclude(rfp => rfp.Form)
                        .ThenInclude(f => f.FormModule)
                            .ThenInclude(fm => fm.Module)
                .Where(r => r.active && r.User.Any(u => u.Id == userId))
                .FirstOrDefaultAsync();

            return rol; // may be null if no active role associated with the user
        }
    }
}