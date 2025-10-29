using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class PersonController : GenericController<PersonDto, Person>
    {
        public PersonController(IPersonBusiness business, ILogger<PersonController> logger)
            : base(business, logger) { }

        protected override int GetEntityId(PersonDto dto) => dto.Id;
    }
}