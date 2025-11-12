using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class CategoriaController : GenericController<CategoriaDto, Categoria>
    {
        public CategoriaController(ICategoriaBusiness business, ILogger<CategoriaController> logger)
            : base(business, logger) { }

        protected override int GetEntityId(CategoriaDto dto) => dto.Id;
    }
}
