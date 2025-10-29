using AutoMapper;
using Entity.Dto;
using Entity.Model;


namespace Utilities.Mappers.Profiles
{
    public class FormModuleProfile : Profile
    {
        public FormModuleProfile()
        {
            CreateMap<FormModule, FormModuleDto>().ReverseMap();
        }
    }
}