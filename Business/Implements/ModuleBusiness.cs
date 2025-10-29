using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public class ModuleBusiness : BaseBusiness<Module, ModuleDto>, IModuleBusiness
    {
        public ModuleBusiness(IModuleData data, IMapper mapper, ILogger<ModuleBusiness> logger)
            : base(data, mapper, logger) { }
    }
}