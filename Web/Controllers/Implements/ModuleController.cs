using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class ModuleController : GenericController<ModuleDto, Module>
    {
        public ModuleController(IModuleBusiness business, ILogger<ModuleController> logger)
            : base(business, logger) { }

        protected override int GetEntityId(ModuleDto dto) => dto.Id;
    }
}