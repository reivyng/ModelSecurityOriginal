using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public class ProductoProveedorBusiness : BaseBusiness<ProductoProveedor, ProductoProveedorDto>, IProductoProveedorBusiness
    {
        public ProductoProveedorBusiness(IProductoProveedorData data, IMapper mapper, ILogger<ProductoProveedorBusiness> logger)
            : base(data, mapper, logger) { }
    }
}
