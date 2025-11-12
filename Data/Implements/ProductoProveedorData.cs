using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Implements
{
    public class ProductoProveedorData : BaseData.BaseModelData<ProductoProveedor>, IProductoProveedorData
    {
        public ProductoProveedorData(ApplicationDbContext context) : base(context) { }
    }
}
