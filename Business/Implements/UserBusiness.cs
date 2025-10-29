using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Implements
{
    public class UserBusiness : BaseBusiness<User, UserDto>, IUserBusiness
    {
        public UserBusiness(IUserData data, IMapper mapper, ILogger<UserBusiness> logger)
            : base(data, mapper, logger) { }
        // Métodos adicionales específicos de User

        public async Task<List<UserDto>> GetAllAsync(bool? active, string? emailSearch)
        {
            var userData = _data as IUserData;
            if (userData == null)
                throw new InvalidOperationException("El data layer no implementa IUserData");

            var users = await userData.GetAllAsync(active, emailSearch);
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}