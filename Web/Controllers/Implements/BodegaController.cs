using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class BodegaController : GenericController<BodegaDto, Bodega>
    {
        public BodegaController(IBodegaBusiness business, ILogger<BodegaController> logger)
            : base(business, logger) { }

        protected override int GetEntityId(BodegaDto dto) => dto.Id;
    }
}
