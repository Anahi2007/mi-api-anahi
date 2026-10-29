using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIEN_LAN_ZU_SHI.Business.DTOs;
using TIEN_LAN_ZU_SHI.Business.Interfaces;
using TIEN_LAN_ZU_SHI.Core.Common;
using TIEN_LAN_ZU_SHI.Core.Entities;
using TIEN_LAN_ZU_SHI.DataAccess.Interfaces;
using TIEN_LAN_ZU_SHI.DataAccess.Repositories;

namespace TIEN_LAN_ZU_SHI.Business.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _ProductRepository;

        //Constructor del Servicio 
        public InventoryService(IInventoryRepository inventoryRepository, IProductRepository productRepository)
        {
            _inventoryRepository = inventoryRepository;
            _ProductRepository = productRepository;
        }

        //Programar la logica /reglas del negocio relacionadas a Categorias
        public async Task<ServiceResponse<IEnumerable<Inventory>>> GetAllAsync()
        {
            var result = await _inventoryRepository.GetAllAsync();

            if (result.OperationStatusCode == 0)
            {
                return new ServiceResponse<IEnumerable<Inventory>>()
                {
                    Data = result.Data,
                    IsSuccess = true,
                    MessageCode = MessageCodes.Sucess,
                    Message = "Operacion exitosa"
                };
            }



            switch (result.OperationStatusCode)
            {
                case 5027:
                    return new ServiceResponse<IEnumerable<Inventory>>
                    {
                        Data = result.Data,
                        IsSuccess = true,
                        MessageCode = MessageCodes.NoData,
                    };

                default:
                    return new ServiceResponse<IEnumerable<Inventory>>
                    {
                        Data = null,
                        IsSuccess = false,
                        MessageCode = MessageCodes.ErrorDataBase,
                        Message = "Ocurrio un error inesperado"
                    };
            }

        }

        public async Task<ServiceResponse<Inventory>> CreateAsync(CreateInventoryDto newInventory)
        {
            try
            {
                //Validar la existencia del Producto en el catalogo
                var existentProduct = await _ProductRepository.GetByIdAsync(newInventory.ProductId);

                if (existentProduct.Data == null)
                {
                    return new ServiceResponse<Inventory>
                    {
                        Data = null,
                        IsSuccess = false,
                        MessageCode = MessageCodes.Conflict,
                        Message = "No existe producto con el id proporcionado"
                    };
                }

                //Traer los datos más recientes ddel producto en el inventario
                var dataInventoryProduct = await _inventoryRepository.GetProductByIdAsync(newInventory.ProductId);

                //Si el producto existe en el inventario capturar su AccumulatedStock
                var AccumulatedStock = 0;
                if (dataInventoryProduct.Data! == null)
                    AccumulatedStock = 0;
                else
                {
                    AccumulatedStock = dataInventoryProduct.Data!.AccumulatedStock;

                    //Validamos si ya tiene stock registrado hoy
                    var existentDate = await _inventoryRepository.GetByProductAndDateAsync(
                        newInventory.ProductId,
                        newInventory.Date
                    );

                    if (existentDate.Data != null)
                    {
                        return new ServiceResponse<Inventory>
                        {
                            Data = null,
                            IsSuccess = false,
                            MessageCode = MessageCodes.Conflict,
                            Message = "Ya existe un registro de inventario para este producto en la fecha proporcionada"
                        };
                    }
                }
                    

                var inventory = new Inventory()
                {
                    ProductId = newInventory.ProductId,
                    Date = newInventory.Date,
                    DailyStock = newInventory.DailyStock,
                    SalePrice = newInventory.SalePrice,
                    AccumulatedStock = 0
                };

                //Sumamos el DaylyStock si el producto es Reconsumible
                if (existentProduct.Data!.Reconsumable == true)
                {
                    inventory.AccumulatedStock = AccumulatedStock + newInventory.DailyStock;

                    if (inventory.AccumulatedStock > 30)
                    {
                        return new ServiceResponse<Inventory>
                        {
                            Data = null,
                            IsSuccess = false,
                            MessageCode = MessageCodes.Conflict,
                            Message = $"El stock acumulado del producto con Id {newInventory.ProductId} superará el limite de stock acumulado (30)"
                        };
                    }
                }
                  
                else
                    inventory.AccumulatedStock = newInventory.DailyStock;

              
                //Invocamos al repo               
                var result = await _inventoryRepository.AddAsync(inventory);

                return new ServiceResponse<Inventory>
                {
                    Data = result.Data,
                    IsSuccess = true,
                    MessageCode = MessageCodes.Sucess,
                    Message = "Inventario registrado"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Inventory>
                {
                    Data = null,
                    IsSuccess = false,
                    MessageCode = MessageCodes.ErrorDataBase,
                    Message = "Ocurrió un error inesperado al registrar el inventario: " + ex.Message
                };
            }
        }

        public async Task<ServiceResponse<Inventory>> GetByProductAndDateAsync(int productId, DateTime date)
        {
            try
            {

                var result = await _inventoryRepository.GetByProductAndDateAsync(productId, date);

                if (result.Data != null)
                {

                    return new ServiceResponse<Inventory>
                    {
                        Data = result.Data,
                        IsSuccess = true,
                        MessageCode = MessageCodes.Sucess,
                        Message = "Inventario encontrado"
                    };
                }
                else
                {

                    return new ServiceResponse<Inventory>
                    {
                        Data = null,
                        IsSuccess = false,
                        MessageCode = MessageCodes.NotFound,
                        Message = "No se encontró inventario para este producto y fecha"
                    };
                }
            }
            catch (Exception ex)
            {

                return new ServiceResponse<Inventory>
                {
                    Data = null,
                    IsSuccess = false,
                    MessageCode = MessageCodes.ErrorDataBase,
                    Message = "Ocurrió un error al consultar el inventario: " + ex.Message
                };
            }
        }

        public async Task<ServiceResponse<Inventory>> GetProductByIdAsync(int id)
        {
            var repoResponse = await _inventoryRepository.GetProductByIdAsync(id);

            try
            {
                switch (repoResponse.OperationStatusCode)
                {
                    case 0: 
                        return new ServiceResponse<Inventory>
                        {
                            Data = repoResponse.Data,
                            IsSuccess = true,
                            MessageCode = MessageCodes.Sucess,
                            Message = repoResponse.Message ?? "Operación exitosa"
                        };

                    case -1: 
                        return new ServiceResponse<Inventory>
                        {
                            Data = null,
                            IsSuccess = false,
                            MessageCode = MessageCodes.NotFound,
                            Message = repoResponse.Message ?? "No se encontró el producto"
                        };

                    default: 
                        return new ServiceResponse<Inventory>
                        {
                            Data = null,
                            IsSuccess = false,
                            MessageCode = MessageCodes.ErrorDataBase,
                            Message = repoResponse.Message ?? "Ocurrió un error inesperado"
                        };
                }
            }
            catch (Exception)
            {
                return new ServiceResponse<Inventory>
                {
                    Data = null,
                    IsSuccess = false,
                    MessageCode = MessageCodes.ErrorDataBase,
                    Message = repoResponse.Message ?? "Ocurrió un error inesperado"
                };
            }
        }

    }
}
