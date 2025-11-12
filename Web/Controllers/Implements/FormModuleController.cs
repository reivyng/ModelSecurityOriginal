using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class FormModuleController : GenericController<FormModuleDto, FormModule>
    {
        private readonly IFormModuleBusiness _business;

        public FormModuleController(IFormModuleBusiness business, ILogger<FormModuleController> logger)
            : base(business, logger)
        {
            _business = business;
        }

        protected override int GetEntityId(FormModuleDto dto) => dto.Id;
    }
}