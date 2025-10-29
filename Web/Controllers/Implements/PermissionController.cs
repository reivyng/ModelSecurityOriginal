using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class PermissionController : GenericController<PermissionDto, Permission>
    {
        public PermissionController(IPermissionBusiness business, ILogger<PermissionController> logger)
            : base(business, logger) { }

        protected override int GetEntityId(PermissionDto dto) => dto.Id;
    }
}