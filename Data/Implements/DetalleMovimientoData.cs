using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Implements
{
    public class DetalleMovimientoData : BaseData.BaseModelData<DetalleMovimiento>, IDetalleMovimientoData
    {
        public DetalleMovimientoData(ApplicationDbContext context) : base(context) { }
    }
}
