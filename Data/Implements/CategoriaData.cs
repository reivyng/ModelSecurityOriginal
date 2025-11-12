using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Implements
{
    public class CategoriaData : BaseData.BaseModelData<Categoria>, ICategoriaData
    {
        public CategoriaData(ApplicationDbContext context) : base(context) { }
    }
}
