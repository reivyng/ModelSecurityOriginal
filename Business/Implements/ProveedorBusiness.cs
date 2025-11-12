using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public class ProveedorBusiness : BaseBusiness<Proveedor, ProveedorDto>, IProveedorBusiness
    {
        public ProveedorBusiness(IProveedorData data, IMapper mapper, ILogger<ProveedorBusiness> logger)
            : base(data, mapper, logger) { }
    }
}
