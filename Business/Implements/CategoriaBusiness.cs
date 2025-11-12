using AutoMapper;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.Extensions.Logging;

namespace Business.Implements
{
    public class CategoriaBusiness : BaseBusiness<Categoria, CategoriaDto>, ICategoriaBusiness
    {
        public CategoriaBusiness(ICategoriaData data, IMapper mapper, ILogger<CategoriaBusiness> logger)
            : base(data, mapper, logger) { }
    }
}
