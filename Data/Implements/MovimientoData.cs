using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Implements
{
    public class MovimientoData : BaseData.BaseModelData<Movimiento>, IMovimientoData
    {
        public MovimientoData(ApplicationDbContext context) : base(context) { }
    }
}
