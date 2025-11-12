using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public class BodegaBusiness : BaseBusiness<Bodega, BodegaDto>, IBodegaBusiness
    {
        public BodegaBusiness(IBodegaData data, IMapper mapper, ILogger<BodegaBusiness> logger)
            : base(data, mapper, logger) { }
    }
}
