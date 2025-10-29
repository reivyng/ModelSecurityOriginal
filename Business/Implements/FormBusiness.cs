using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public class FormBusiness : BaseBusiness<Form, FormDto>, IFormBusiness
    {
        public FormBusiness(IFormData data, IMapper mapper, ILogger<FormBusiness> logger)
            : base(data, mapper, logger) { }
    }
}