using AutoMapper;
using Entity.Model;
using Entity.Dto;

namespace Utilities.Mappers.Profiles
{
    public class InventarioProfile : Profile
    {
        public InventarioProfile()
        {
            CreateMap<Bodega, BodegaDto>().ReverseMap();
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<Proveedor, ProveedorDto>().ReverseMap();
            CreateMap<ProductoProveedor, ProductoProveedorDto>().ReverseMap();
            CreateMap<Movimiento, MovimientoDto>().ReverseMap();
            CreateMap<DetalleMovimiento, DetalleMovimientoDto>().ReverseMap();
        }
    }
}
