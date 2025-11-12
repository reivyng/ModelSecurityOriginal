using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class DetalleMovimientoController : GenericController<DetalleMovimientoDto, DetalleMovimiento>
    {
        public DetalleMovimientoController(IDetalleMovimientoBusiness business, ILogger<DetalleMovimientoController> logger)
            : base(business, logger) { }

        protected override int GetEntityId(DetalleMovimientoDto dto) => dto.Id;
    }
}
