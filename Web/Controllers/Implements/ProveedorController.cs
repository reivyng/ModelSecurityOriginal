using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class ProveedorController : GenericController<ProveedorDto, Proveedor>
    {
        public ProveedorController(IProveedorBusiness business, ILogger<ProveedorController> logger)
            : base(business, logger) { }

        protected override int GetEntityId(ProveedorDto dto) => dto.Id;
    }
}
