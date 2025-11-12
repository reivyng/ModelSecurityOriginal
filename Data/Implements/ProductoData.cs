using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Implements
{
    public class ProductoData : BaseData.BaseModelData<Producto>, IProductoData
    {
        public ProductoData(ApplicationDbContext context) : base(context) { }
    }
}
