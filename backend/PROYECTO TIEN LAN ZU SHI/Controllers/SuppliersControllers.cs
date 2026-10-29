using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROYECTO_TIEN_LAN_ZU_SHI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using TIEN_LAN_ZU_SHI.Business.DTOs;
using TIEN_LAN_ZU_SHI.Core.Common;
using TIEN_LAN_ZU_SHI.Core.Entities;
using TIEN_LAN_ZU_SHI.Services;

namespace TIEN_LAN_ZU_SHI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly SupplierService _supplierService;

        public SupplierController(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [Authorize(Roles = "Administrador,Cajero")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _supplierService.GetAllSuppliersAsync();


            if (response.IsSuccess)
            {
                var dtoList = response.Data.Select(s => new SupplierDto
                {
                    Id = s.Id,
                    NombreProveedor = s.NombreProveedor,
                    Contacto = s.Contacto,
                    Ciudad = s.Ciudad,
                    Telefono = s.Telefono,
                    Correo = s.Correo,
                    Direccion = s.Direccion,
                    FechaRegistro = s.FechaRegistro,
                    
                });

                return Ok(new ApiResponse<IEnumerable<SupplierDto>>
                {
                    Data = dtoList,
                    Meta = new { total = dtoList.Count(), message = response.Message }
                });
            }

            return Ok(new UnsuccessfulResponseDto
            {
                Code = "200",
                Message = "No se encontraron proveedores",
                Details = new { info = "La lista está vacía" }
            });
        }





        [Authorize(Roles = "Administrador,Cajero")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _supplierService.GetByIdAsync(id);

            if (response.IsSuccess)
            {
                var s = response.Data;
                var dto = new SupplierDto
                {
                    Id = s.Id,
                    NombreProveedor = s.NombreProveedor,
                    Contacto = s.Contacto,
                    Ciudad = s.Ciudad,
                    Telefono = s.Telefono,
                    Correo = s.Correo,
                    Direccion = s.Direccion,
                    FechaRegistro = s.FechaRegistro,
                    
                };

                return Ok(new ApiResponse<SupplierDto> { Data = dto, Meta = new { message = response.Message } });
            }

            return NotFound(new UnsuccessfulResponseDto
            {
                Code = "404",
                Message = "Proveedor no encontrado",
                Details = new { info = "No existe proveedor con ese ID" }
            });
        }



        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] CreateSupplierDto dto)
        {
            var response = await _supplierService.CreateAsync(dto);

            if (response.IsSuccess)
            {
                return Ok(new ApiResponse<string>
                {
                    Data = "Proveedor creado exitosamente",
                    Meta = new { message = response.Message }
                });
            }

            return BadRequest(new UnsuccessfulResponseDto
            {
                Code = "400",
                Message = "Error al crear proveedor",
                Details = new { info = response.Message }
            });
        }


        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSupplierDto dto)
        {
            var response = await _supplierService.UpdateAsync(id, dto);

            if (response.IsSuccess)
            {
                return Ok(new ApiResponse<string> { Data = "Proveedor actualizado correctamente" });
            }

            return Conflict(new UnsuccessfulResponseDto
            {
                Code = "409",
                Message = "Conflicto al actualizar proveedor",
                Details = new { info = response.Message }
            });
        }
        [Authorize(Roles = "Administrador")]
        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> UpdateState(int id, [FromBody] string nuevoEstado)
        {
            var response = await _supplierService.UpdateStateAsync(id, nuevoEstado);

            if (response.IsSuccess)
            {
                return Ok(new ApiResponse<string> { Data = "Estado del proveedor actualizado" });
            }

            return BadRequest(new UnsuccessfulResponseDto
            {
                Code = "400",
                Message = "No se pudo cambiar el estado",
                Details = new { info = response.Message }
            });
        }
    }
}

          

