using AutoMapper;
using Business.Interfaces;
using Data.Interface;
using Data.Interfaces;
using Entity.Dto;
using Entity.Model;

using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public class PersonBusiness : BaseBusiness<Person, PersonDto>, IPersonBusiness
    {
        public PersonBusiness(IPersonData data, IMapper mapper, ILogger<PersonBusiness> logger)
            : base(data, mapper, logger) { }
    }
}