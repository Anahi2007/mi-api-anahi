using System.Collections.Generic;
using System.Threading.Tasks;
using TIEN_LAN_ZU_SHI.Core.Entities;
using TIEN_LAN_ZU_SHI.DataAccess.Interfaces;

namespace TIEN_LAN_ZU_SHI.Services
{
    public class SupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return _supplierRepository.GetAllAsync();
        }

        public Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            return _supplierRepository.GetByIdAsync(id);
        }

        public Task AddSupplierAsync(Supplier supplier)
        {
            return _supplierRepository.AddAsync(supplier);
        }

        public Task UpdateSupplierAsync(Supplier supplier)
        {
            return _supplierRepository.UpdateAsync(supplier);
        }

        public Task DeleteSupplierAsync(int id)
        {
            return _supplierRepository.DeleteAsync(id);
        }
    }
}


