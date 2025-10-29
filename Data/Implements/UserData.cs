using Data.Interfaces;
using Entity.Context;
using Entity.Model;

namespace Data.Implements
{
    public class UserData : BaseData.BaseModelData<User>, IUserData
    {
        public UserData(ApplicationDbContext context) : base(context) { }
        // Métodos adicionales específicos de User
    }
}