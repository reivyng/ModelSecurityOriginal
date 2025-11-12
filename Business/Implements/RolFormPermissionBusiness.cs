using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto;
using Entity.Dto.Menu;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Implements
{
    public class RolFormPermissionBusiness : BaseBusiness<RolFormPermission, RolFormPermissionDto>, IRolFormPermissionBusiness
    {
        private readonly IRolFormPermissionData _rfpData;

        public RolFormPermissionBusiness(IRolFormPermissionData data, IMapper mapper, ILogger<RolFormPermissionBusiness> logger)
            : base(data, mapper, logger)
        {
            _rfpData = data;
        }

        public async Task<List<MenuDto>> ObtenerMenu(int userId)
        {
            var rol = await _rfpData.ObtenerMenu(userId);
            var result = new List<MenuDto>();
            if (rol == null) return result;

            var menuDto = new MenuDto
            {
                Rol = rol.type_rol,
                ModuleForm = new List<MenuFormDto>()
            };

            var moduleGroups = new Dictionary<int, MenuFormDto>();

            foreach (var rfp in rol.RolFormPermission ?? new List<RolFormPermission>())
            {
                if (rfp == null || !rfp.active) continue;
                var form = rfp.Form;
                if (form == null || !form.active) continue;

                foreach (var fm in form.FormModule ?? new List<FormModule>())
                {
                    if (fm == null || !fm.active) continue;
                    var module = fm.Module;
                    if (module == null || !module.active) continue;

                    if (!moduleGroups.TryGetValue(module.Id, out var mfDto))
                    {
                        mfDto = new MenuFormDto { Id = module.Id, Name = module.name, Form = new List<FormItemDto>() };
                        moduleGroups[module.Id] = mfDto;
                    }

                    if (!mfDto.Form.Any(x => x.Name == form.name))
                    {
                        mfDto.Form.Add(new FormItemDto { Name = form.name, Path = form.path });
                    }
                }
            }

            menuDto.ModuleForm = moduleGroups.Values.ToList();
            result.Add(menuDto);

            if (result.All(m => m.ModuleForm == null || !m.ModuleForm.Any()))
            {
                return await ObtenerMenuFallback(userId);
            }

            return result;
        }

        public async Task<List<MenuDto>> ObtenerMenuFallback(int userId)
        {
            var rol = await _rfpData.ObtenerMenu(userId);
            var result = new List<MenuDto>();
            if (rol == null) return result;

            var menuDto = new MenuDto { Rol = rol.type_rol, ModuleForm = new List<MenuFormDto>() };
            var moduleGroups = new Dictionary<int, MenuFormDto>();

            foreach (var rfp in rol.RolFormPermission ?? new List<RolFormPermission>())
            {
                var form = rfp?.Form;
                if (form == null) continue;

                foreach (var fm in form.FormModule ?? new List<FormModule>())
                {
                    var module = fm?.Module;
                    if (module == null) continue;

                    if (!moduleGroups.TryGetValue(module.Id, out var mfDto))
                    {
                        mfDto = new MenuFormDto { Id = module.Id, Name = module.name, Form = new List<FormItemDto>() };
                        moduleGroups[module.Id] = mfDto;
                    }

                    if (!mfDto.Form.Any(x => x.Name == form.name))
                    {
                        mfDto.Form.Add(new FormItemDto { Name = form.name, Path = form.path });
                    }
                }
            }

            menuDto.ModuleForm = moduleGroups.Values.ToList();
            result.Add(menuDto);
            return result;
        }
    }
}