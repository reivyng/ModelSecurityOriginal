using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class MovimientoController : GenericController<MovimientoDto, Movimiento>
    {
        public MovimientoController(IMovimientoBusiness business, ILogger<MovimientoController> logger)
            : base(business, logger) { }

        protected override int GetEntityId(MovimientoDto dto) => dto.Id;
    }
}
