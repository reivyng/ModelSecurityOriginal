using System.Collections.Generic;
using System.Threading.Tasks;
using Entity.Dto;
using Entity.Model;

namespace Business.Interfaces
{
    public interface IBaseBusiness<T, D> where T : BaseModel where D : BaseDto
    {
        Task<List<D>> GetAllAsync();
        Task<D> GetByIdAsync(int id);
        Task<D> CreateAsync(D dto);
        Task<D> UpdateAsync(D dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> SoftDeleteAsync(int id);
    }
}
