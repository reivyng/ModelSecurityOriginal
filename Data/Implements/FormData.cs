using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Implements
{
    public class FormData : BaseData.BaseModelData<Form>, IFormData
    {
        public FormData(ApplicationDbContext context) : base(context) { }
    }
}