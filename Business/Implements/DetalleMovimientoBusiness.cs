using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public class DetalleMovimientoBusiness : BaseBusiness<DetalleMovimiento, DetalleMovimientoDto>, IDetalleMovimientoBusiness
    {
        public DetalleMovimientoBusiness(IDetalleMovimientoData data, IMapper mapper, ILogger<DetalleMovimientoBusiness> logger)
            : base(data, mapper, logger) { }
    }
}
