using Business.Interfaces;
using Entity.Dto;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;


namespace Web.Controllers.Implements
{
    [ApiController]
    public abstract class GenericController<TDto, TEntity> : ControllerBase
    where TEntity : BaseModel
    where TDto : BaseDto
    {
        protected readonly IBaseBusiness<TEntity, TDto> _business;
        protected readonly ILogger<GenericController<TDto, TEntity>> _logger;

        protected GenericController(IBaseBusiness<TEntity, TDto> business, ILogger<GenericController<TDto, TEntity>> logger)
        {
            _business = business;
            _logger = logger;
        }
   




        [HttpGet]
        public virtual async Task<IActionResult> GetAll()
        {
            try
            {
                var entities = await _business.GetAllAsync();
                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener todos los registros: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            try
            {
                var entity = await _business.GetByIdAsync(id);
                if (entity == null)
                    return NotFound($"Registro con ID {id} no encontrado");

                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener registro con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] TDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdEntity = await _business.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = GetEntityId(createdEntity) }, createdEntity);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al crear registro: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear registro: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut]
        public virtual async Task<IActionResult> Update([FromBody] TDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedEntity = await _business.UpdateAsync(dto);
                return Ok(updatedEntity);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar registro: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar registro: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch("{id}")]
        public virtual async Task<IActionResult> Patch([FromRoute] int id, [FromBody] TDto patchDto)
        {
            try
            {
                // Obtener el body crudo para saber qué propiedades fueron enviadas
                string rawBody = string.Empty;
                using (var reader = new StreamReader(Request.Body))
                {
                    Request.Body.Position = 0;
                    rawBody = await reader.ReadToEndAsync();
                }
                var presentProps = new List<string>();
                if (!string.IsNullOrEmpty(rawBody))
                {
                    var json = System.Text.Json.JsonDocument.Parse(rawBody);
                    foreach (var prop in json.RootElement.EnumerateObject())
                    {
                        presentProps.Add(prop.Name);
                    }
                }

                var result = await _business.PatchAsync(id, patchDto, presentProps);
                if (!result)
                    return NotFound(new { message = "Entidad no encontrada o no se pudo actualizar." });

                return NoContent();
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno.", detail = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _business.DeleteAsync(id);
                if (!result)
                    return NotFound($"Registro con ID {id} no encontrado");

                return Ok(new { Success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar registro con ID {id}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
        /// <summary>
        /// Elimina lógicamente un registro por su ID.
        /// </summary>
        /// <param name="id">ID del registro a eliminar</param>
        [HttpDelete("{id}/deleteLogical")]
        public async Task<IActionResult> SoftActive(int id)
        {
            var result = await _business.SoftDeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // Método abstracto para obtener el ID de la entidad creada para el CreatedAtAction
        protected abstract int GetEntityId(TDto dto);
    }
}