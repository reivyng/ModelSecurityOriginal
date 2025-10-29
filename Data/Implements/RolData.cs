using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Implements
{
    public class RolData : BaseData.BaseModelData<Rol>, IRolData
    {
        public RolData(ApplicationDbContext context) : base(context) { }
    }
}