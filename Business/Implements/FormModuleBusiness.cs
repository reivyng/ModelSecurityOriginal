using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public class FormModuleBusiness : BaseBusiness<FormModule, FormModuleDto>, IFormModuleBusiness
    {
        public FormModuleBusiness(IFormModuleData data, IMapper mapper, ILogger<FormModuleBusiness> logger)
            : base(data, mapper, logger) { }
    }
}