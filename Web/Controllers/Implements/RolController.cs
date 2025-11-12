using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class RolController : GenericController<RolDto, Rol>
    {
        private readonly IRolBusiness _rolBusiness;

        public RolController(IRolBusiness rolBusiness, ILogger<RolController> logger)
            : base(rolBusiness, logger)
        {
            _rolBusiness = rolBusiness;
        }

        protected override int GetEntityId(RolDto dto) => dto.Id;
    }
}