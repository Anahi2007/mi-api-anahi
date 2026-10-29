using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TIEN_LAN_ZU_SHI.Core.Entities;
using TIEN_LAN_ZU_SHI.DataAccess.Interfaces;

namespace TIEN_LAN_ZU_SHI.DataAccess.Repositories
{
    public class SupplierRepository
    {
        private readonly string _connectionString;

        public SupplierRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Supplier>> GetAllAsync()
        {
            var suppliers = new List<Supplier>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Suppliers", conn);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var supplier = new Supplier
                        {
                            Id = reader.GetInt32(0),
                            NombreProveedor = reader.GetString(1),
                            Contacto = reader.GetString(2),
                            Ciudad = reader.GetString(3),
                            Telefono = reader.GetString(4),
                            Correo = reader.GetString(5),
                            Direccion = reader.GetString(6),
                            FechaRegistro = reader.GetDateTime(7)
                        };
                        suppliers.Add(supplier);
                    }
                }
            }

            return suppliers;
        }
    }
}


