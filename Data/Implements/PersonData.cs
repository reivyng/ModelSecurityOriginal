using Data.Interface;
using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Implements
{
    public class PersonData : BaseData.BaseModelData<Person>, IPersonData
    {
        public PersonData(ApplicationDbContext context) : base(context) { }
    }
}