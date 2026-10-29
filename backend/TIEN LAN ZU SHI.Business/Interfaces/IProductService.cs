using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIEN_LAN_ZU_SHI.Business.DTOs;
using TIEN_LAN_ZU_SHI.Core.Common;
using TIEN_LAN_ZU_SHI.Core.Entities;

namespace TIEN_LAN_ZU_SHI.Business.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResponse<IEnumerable<Product>>> GetAllAsync();
         Task<ServiceResponse<Product>> GetByIdAsync(int ids);
        Task<ServiceResponse<Product>> GetByNameAsync(string name);
        Task<ServiceResponse<Product>> CreateAsync(CreateProductDto newProduct);
        Task<ServiceResponse<Product>> UpdateAsync(int id, UpdateProductDto product);
        Task<ServiceResponse<Product>> SetStateAsync(int productId, bool state);
        Task<ServiceResponse<Product>> SetReconsumableAsync(int productId, bool state);
    }
}
