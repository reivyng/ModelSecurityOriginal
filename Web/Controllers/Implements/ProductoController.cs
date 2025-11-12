using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class ProductoController : GenericController<ProductoDto, Producto>
    {
        public ProductoController(IProductoBusiness business, ILogger<ProductoController> logger)
            : base(business, logger) { }

        protected override int GetEntityId(ProductoDto dto) => dto.Id;
    }
}
