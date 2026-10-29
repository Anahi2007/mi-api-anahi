using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIEN_LAN_ZU_SHI.Core.Common;
using TIEN_LAN_ZU_SHI.Core.Entities;

namespace TIEN_LAN_ZU_SHI.DataAccess.Interfaces
{
    public interface IInventoryRepository
    {
        Task<RepositoryResponse<IEnumerable<Inventory>>>GetAllAsync();
        Task<RepositoryResponse<Inventory>> AddAsync(Inventory inventory);
        Task<RepositoryResponse<Inventory>> GetByProductAndDateAsync(int productId, DateTime date);
        Task<RepositoryResponse<Inventory>> GetProductByIdAsync(int id);



        //Task<RepositoryResponse<Inventory>>GetByIdAsync(int id);


        //Task<RepositoryResponse<int>>Addsync(Inventory inventory);


        //Task<RepositoryResponse<int>>Updatesync(Inventory inventory);


        //Task<bool> Desactive(int id);
    }
}
