using Data.Implements.BaseData;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Data.Implements.BaseData
{
    public class BaseModelData<T> : ABaseModelData<T> where T : BaseModel
    {
        public BaseModelData(ApplicationDbContext context) : base(context)
        {
        }

        // Implementación completa de los métodos abstractos
        public override async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public override async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public override async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Actualiza parcialmente una entidad (PATCH) aplicando solo los campos modificados.
        /// </summary>
        /// <param name="id">Identificador de la entidad.</param>
        /// <param name="patchValues">Acción que aplica los cambios sobre la entidad encontrada.</param>
        public override async Task<bool> PatchAsync(object id, Action<T> patchValues)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
                return false;

            patchValues(entity);
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }


        public override  async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) return false;

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Cambia el estado activo/inactivo de la entidad (Toggle)
        /// </summary>
        /// <param name="id">ID de la entidad</param>
        /// <returns>True si se cambió el estado correctamente, False si no se encontró</returns>
        public override async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            // Toggle: Si está activo lo desactiva, si está inactivo lo activa
            entity.active = !entity.active;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
