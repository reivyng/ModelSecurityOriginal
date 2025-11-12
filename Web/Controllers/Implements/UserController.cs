using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class UserController : GenericController<UserDto, User>
    {
        public UserController(IUserBusiness business, ILogger<UserController> logger)
            : base(business, logger) { }

        protected override int GetEntityId(UserDto dto)
        {
            return dto.Id;
        }
    }
}