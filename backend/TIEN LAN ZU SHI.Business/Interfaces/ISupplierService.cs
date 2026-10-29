using System.Collections.Generic;
using System.Threading.Tasks;
using TIEN_LAN_ZU_SHI.Business.DTOs;

namespace TIEN_LAN_ZU_SHI.Business.Interfaces
{
    public interface ISupplierService
    {
        Task<List<SupplierDto>> GetAllAsync();
        Task<SupplierDto> GetByIdAsync(int id);
        Task CreateAsync(CreateSupplierDto dto);
        Task UpdateAsync(SupplierDto dto);
        Task DeleteAsync(int id);
    }
}


