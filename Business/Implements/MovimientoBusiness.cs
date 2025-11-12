using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public class MovimientoBusiness : BaseBusiness<Movimiento, MovimientoDto>, IMovimientoBusiness
    {
        public MovimientoBusiness(IMovimientoData data, IMapper mapper, ILogger<MovimientoBusiness> logger)
            : base(data, mapper, logger) { }
    }
}
