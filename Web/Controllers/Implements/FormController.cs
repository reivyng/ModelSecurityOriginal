using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class FormController : GenericController<FormDto, Form>
    {
        private readonly IFormBusiness _formBusiness;

        public FormController(IFormBusiness formBusiness, ILogger<FormController> logger)
            : base(formBusiness, logger)
        {
            _formBusiness = formBusiness;
        }

        protected override int GetEntityId(FormDto dto) => dto.Id;
    }
}