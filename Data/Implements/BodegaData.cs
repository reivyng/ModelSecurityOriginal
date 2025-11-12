using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Implements
{
    public class BodegaData : BaseData.BaseModelData<Bodega>, IBodegaData
    {
        public BodegaData(ApplicationDbContext context) : base(context) { }
    }
}
