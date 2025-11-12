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
        private readonly IPermissionBusiness _business;

        public PermissionController(IPermissionBusiness business, ILogger<PermissionController> logger)
            : base(business, logger)
        {
            _business = business;
        }

        protected override int GetEntityId(PermissionDto dto) => dto.Id;
    }
}