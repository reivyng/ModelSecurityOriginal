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
        private readonly IModuleBusiness _moduleBusiness;

        public ModuleController(IModuleBusiness moduleBusiness, ILogger<ModuleController> logger)
            : base(moduleBusiness, logger)
        {
            _moduleBusiness = moduleBusiness;
        }

        protected override int GetEntityId(ModuleDto dto) => dto.Id;
    }
}