using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Implements
{
    public class ModuleData : BaseData.BaseModelData<Module>, IModuleData
    {
        public ModuleData(ApplicationDbContext context) : base(context) { }
    }
}