using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TIEN_LAN_ZU_SHI.Core.Common;
using TIEN_LAN_ZU_SHI.Core.Entities;
using TIEN_LAN_ZU_SHI.DataAccess.Interfaces;

namespace TIEN_LAN_ZU_SHI.DataAccess.Repositories
{
   public  class ProductRepository: IProductRepository
    {
        private readonly string _connectionString;
        public ProductRepository (IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<RepositoryResponse<IEnumerable<Product>>> GetAllAsync()
        {
            var Products = new List<Product>();
            //Instancia de objeto RepositoryResponse que se retornara
            var response = new RepositoryResponse<IEnumerable<Product>>();
            try
            {
                //Establecemos la conexion a la BD
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand cmd = new SqlCommand("GetProducts", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Products.Add(new Product
                            {
                                Id = (int)reader["Id"],
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = reader["CategoryName"].ToString()!,
                                ProductName= reader["ProductName"].ToString()!,
                                PortionPrice = Convert.ToDecimal(reader["PortionPrice"].ToString()),
                                Description= reader["Description"].ToString()!,
                                State= (bool)reader["State"],
                                Reconsumable= (bool)reader["Reconsumable"],
                            });
                        }
                    }

                    //Capturando el valor que retorna  el USP
                    var returnValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);
                    response.Data = Products;
                    response.OperationStatusCode = returnValue;
                   
                }

            }
            catch (SqlException ex)
            {
                response.Data = null;
                response.OperationStatusCode = ex.Number;
            }

            return response;



        }


        public async Task<RepositoryResponse<Product>> GetByIdAsync(int id)
        {
            var productReturned = new Product();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand cmd = new SqlCommand("USP_GetProductById", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            productReturned.Id = (int)reader["Id"];
                            productReturned.CategoryId = (int)reader["CategoryId"];
                            productReturned.CategoryName=reader["CategoryName"].ToString();
                            productReturned.ProductName = reader["ProductName"].ToString();
                            productReturned.PortionPrice = Convert.ToDecimal(reader["PortionPrice"].ToString());
                            productReturned.Description = reader["Description"].ToString();
                            productReturned.State = (bool)reader["State"];
                            productReturned.Reconsumable = (bool)reader["Reconsumable"];


                        }
                    }

                    //capturamos el codigo de retorno
                    var returnedValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);

                    return new RepositoryResponse<Product>
                    {
                        Data = productReturned,
                        OperationStatusCode = returnedValue

                    };


                }
            }
            catch (SqlException ex)
            {
                return new RepositoryResponse<Product>
                {
                    Data = null,
                    OperationStatusCode = ex.Number,
                    Message = ex.Message

                };

            }
        }


        public async Task<RepositoryResponse<Product>> AddAsync(Product product)
        {
            var productReturned = new Product();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand cmd = new SqlCommand("SpInsertProduct", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@PortionPrice", product.PortionPrice);
                    cmd.Parameters.AddWithValue("@Description", product.Description);
                    cmd.Parameters.AddWithValue("@State", product.State);
                    cmd.Parameters.AddWithValue("@Reconsumable", product.Reconsumable);
                    cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            productReturned.Id = (int)reader["Id"];
                            productReturned.CategoryId = (int)reader["CategoryId"];
                            productReturned.ProductName = reader["ProductName"].ToString();
                            productReturned.PortionPrice = Convert.ToDecimal(reader["PortionPrice"].ToString());
                            productReturned.Description = reader["Description"].ToString();
                            productReturned.State = (bool)reader["State"];
                            productReturned.Reconsumable = (bool)reader["Reconsumable"];

                        }
                    }

                    //capturamos el codigo de retorno
                    var returnedValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);

                    return new RepositoryResponse<Product>
                    {
                        Data = productReturned,
                        OperationStatusCode = returnedValue

                    };


                }
            }
            catch (SqlException ex)
            {
                return new RepositoryResponse<Product>
                {
                    Data = null,
                    OperationStatusCode = ex.Number,
                    Message = ex.Message

                };

            }
        }


        public async Task<RepositoryResponse<Product>> GetByNameAsync(string name)
        {
            var Product = new Product();
            var response = new RepositoryResponse<Product>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    SqlCommand cmd = new SqlCommand("USP_GetProductByName", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                    using (var reader = await cmd.ExecuteReaderAsync()) 
                    {
                        if (await reader.ReadAsync())
                        {
                            Product.Id = (int)reader["Id"];
                            Product.CategoryId = (int)reader["CategoryId"];
                            Product.CategoryName = reader["CategoryName"].ToString();
                            Product.ProductName = reader["ProductName"].ToString();
                            Product.PortionPrice = Convert.ToDecimal(reader["PortionPrice"].ToString());
                            Product.Description = reader["Description"].ToString();
                            Product.State = (bool)reader["State"];
                            Product.Reconsumable = (bool)reader["Reconsumable"];
                        }
                    }

                    var returnedValue = Convert.ToInt32(cmd.Parameters["@ReturnValue"].Value);

                    response.Data = Product;
                    response.OperationStatusCode = returnedValue;

                    return response;
                }
            }
            catch (SqlException ex)
            {
                response.Data = null;
                response.OperationStatusCode = ex.Number;
                return response;
            }
        }


        public async Task<RepositoryResponse<Product>> UpdateAsync(int id,Product product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand cmd = new SqlCommand("SpUpdateProduct", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@PortionPrice", product.PortionPrice);
                    cmd.Parameters.AddWithValue("@Description", product.Description);
                    cmd.Parameters.AddWithValue("@State", product.State);
                    cmd.Parameters.AddWithValue("@Reconsumable", product.Reconsumable);

                    Product productUpdated = null;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            productUpdated = new Product
                            {
                                Id = (int)reader["Id"],
                                CategoryId = (int)reader["CategoryId"],
                                ProductName = reader["ProductName"].ToString(),
                                PortionPrice = Convert.ToDecimal(reader["PortionPrice"].ToString()),
                                Description = reader["Description"].ToString(),
                                State = (bool)reader["State"],
                               Reconsumable = (bool)reader["Reconsumable"],
                            };
                        }
                    }

                    return new RepositoryResponse<Product>
                    {
                        Data = productUpdated, ////ME///
                        OperationStatusCode = 0
                    };
                }
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<Product>
                {
                    Data = null,
                    OperationStatusCode = -1,
                    Message = ex.Message
                };
            }
        }


        public async Task<RepositoryResponse<Product>> SetStateAsync(int productId, bool state)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand cmd = new SqlCommand("USP_UpdateProductsState", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", productId);
                    cmd.Parameters.AddWithValue("@State", state);

                    Product productUpdated = null;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            productUpdated = new Product
                            {
                                Id = (int)reader["Id"],
                                CategoryId = (int)reader["CategoryId"],
                                ProductName = reader["ProductName"].ToString(),
                                PortionPrice = Convert.ToDecimal(reader["PortionPrice"]),
                                Description = reader["Description"].ToString(),
                                State = (bool)reader["State"],
                                Reconsumable = (bool)reader["Reconsumable"]
                            };
                        }
                    }

                    return new RepositoryResponse<Product>
                    {
                        Data = productUpdated,
                        OperationStatusCode = productUpdated != null ? 0 : 1
                    };
                }
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<Product>
                {
                    Data = null,
                    OperationStatusCode = -1,
                    Message = ex.Message
                };
            }
        }

        public async Task<RepositoryResponse<Product>> SetReconsumableAsync(int productId, bool state)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand cmd = new SqlCommand("USP_UpdateProductsReconsumable", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", productId);
                    cmd.Parameters.AddWithValue("@Reconsumable", state);

                    Product productUpdated = null;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            productUpdated = new Product
                            {
                                Id = (int)reader["Id"],
                                CategoryId = (int)reader["CategoryId"],
                                ProductName = reader["ProductName"].ToString(),
                                PortionPrice = Convert.ToDecimal(reader["PortionPrice"]),
                                Description = reader["Description"].ToString(),
                                State = (bool)reader["State"],
                                Reconsumable = (bool)reader["Reconsumable"]
                            };
                        }
                    }

                    return new RepositoryResponse<Product>
                    {
                        Data = productUpdated,
                        OperationStatusCode = productUpdated != null ? 0 : 1
                    };
                }
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<Product>
                {
                    Data = null,
                    OperationStatusCode = -1,
                    Message = ex.Message
                };
            }
        }





    }
}


