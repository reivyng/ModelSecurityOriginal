using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class RolFormPermissionController : GenericController<RolFormPermissionDto, RolFormPermission>
    {
        private readonly IRolFormPermissionBusiness _rfpBusiness;

        public RolFormPermissionController(IRolFormPermissionBusiness rfpBusiness, ILogger<RolFormPermissionController> logger)
            : base(rfpBusiness, logger)
        {
            _rfpBusiness = rfpBusiness;
        }

        protected override int GetEntityId(RolFormPermissionDto dto) => dto.Id;


        [HttpGet("menu/{userId}")]
        public async Task<IActionResult> Menu(int userId)
        {
            var menu = await _rfpBusiness.ObtenerMenu(userId);
            return Ok(menu);
        }

    }
}