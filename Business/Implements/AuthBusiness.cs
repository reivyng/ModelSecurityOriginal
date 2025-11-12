using System.Threading.Tasks;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto.Auth;
using Entity.Model;

namespace Business.Implements
{
    public class AuthService : IAuthBusiness
    {
        private readonly IUserData _userData;
        private readonly IRolData _rolData;

        public AuthService(IUserData userData, IRolData rolData)
        {
            _userData = userData;
            _rolData = rolData;
        }

        public async Task<bool> RegisterAsync(RegisterUserDto dto)
        {
            // duplicate check
            var userExists = await _userData.FindByEmailAsync(dto.email);
            if (userExists != null) return false;

            var person = new Person
            {
                first_name = dto.first_name,
                first_last_name = dto.first_last_name,
                phone_number = dto.phone_number,
                number_identification = dto.number_identification
            };

            var user = new User
            {
                email = dto.email,
                password = BCrypt.Net.BCrypt.HashPassword(dto.password),
                Person = person,
                active = true
            };

            // assign role id 1 if available, otherwise create a default role
            var role = await _rolData.GetByIdAsync(1);
            if (role == null)
            {
                role = new Rol { type_rol = "User", description = "Default role", active = true };
                role = await _rolData.CreateAsync(role);
            }

            user.rol_id = role.Id;
            user.Rol = role;

            var created = await _userData.CreateAsync(user);
            return created != null;
        }
    }
}
