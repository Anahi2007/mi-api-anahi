using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIEN_LAN_ZU_SHI.Core.Common;
using TIEN_LAN_ZU_SHI.Core.Entities;

namespace TIEN_LAN_ZU_SHI.DataAccess.Interfaces
{
    public interface IProductRepository
    {
        Task<RepositoryResponse<IEnumerable<Product>>>GetAllAsync();


        Task<RepositoryResponse<Product>>GetByIdAsync(int id);

        Task<RepositoryResponse<Product>> GetByNameAsync(string name);


        Task<RepositoryResponse<Product>> AddAsync(Product product);
        Task<RepositoryResponse<Product>> UpdateAsync(int id, Product product);


        Task<RepositoryResponse<Product>> SetStateAsync(int productId, bool state);

        Task<RepositoryResponse<Product>> SetReconsumableAsync(int productId, bool state);


        //Task<RepositoryResponse<int>>Updatesync(Product product);


        //Task<bool> Desactive(int id);
    }
}
