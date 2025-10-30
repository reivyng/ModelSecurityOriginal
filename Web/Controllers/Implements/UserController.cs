using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class UserController : GenericController<UserDto, User>
    {
        public UserController(IUserBusiness business, ILogger<UserController> logger)
            : base(business, logger) { }

        protected override int GetEntityId(UserDto dto) => dto.Id;

        /// <summary>
        /// Obtiene usuarios filtrados por estado activo y búsqueda por correo.
        /// Ruta: GET api/User/filter
        /// Parámetros de consulta:
        /// - active: filtra por campo `active` del DTO (true/false)
        /// - email: búsqueda por correo electrónico
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] bool? active, [FromQuery] string? email)
        {
            try
            {
                // Delegar la lógica de filtrado a la capa de negocio (solo User)
                var users = await ((IUserBusiness)_business).GetAllAsync(active, email);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener usuarios filtrados: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}