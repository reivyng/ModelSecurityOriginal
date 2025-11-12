using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class ProductoProveedorController : GenericController<ProductoProveedorDto, ProductoProveedor>
    {
        public ProductoProveedorController(IProductoProveedorBusiness business, ILogger<ProductoProveedorController> logger)
            : base(business, logger) { }

        protected override int GetEntityId(ProductoProveedorDto dto) => dto.Id;
    }
}
