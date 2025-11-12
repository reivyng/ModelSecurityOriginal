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
        private readonly IPersonBusiness _personBusiness;

        public PersonController(IPersonBusiness personBusiness, ILogger<PersonController> logger)
            : base(personBusiness, logger)
        {
            _personBusiness = personBusiness;
        }

        protected override int GetEntityId(PersonDto dto) => dto.Id;
    }
}