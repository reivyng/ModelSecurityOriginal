using AutoMapper;
using Entity.Dto;
using Entity.Model;

namespace Utilities.Mappers.Profiles
{
    public class RolFormPermissionProfile : Profile
    {
        public RolFormPermissionProfile()
        {
            CreateMap<RolFormPermission, RolFormPermissionDto>().ReverseMap();
        }
    }
}