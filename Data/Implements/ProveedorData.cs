using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Implements
{
    public class ProveedorData : BaseData.BaseModelData<Proveedor>, IProveedorData
    {
        public ProveedorData(ApplicationDbContext context) : base(context) { }
    }
}
