using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIEN_LAN_ZU_SHI.Business.DTOs
{
    
        public class CreateSupplierDto
        {
            [Required(ErrorMessage = "El nombre del proveedor es obligatorio")]
            public string NombreProveedor { get; set; }

            public string Contacto { get; set; }

            public string Ciudad { get; set; }

            [Phone(ErrorMessage = "Número de teléfono no válido")]
            public string Telefono { get; set; }

            [EmailAddress(ErrorMessage = "Correo electrónico no válido")]
            public string Correo { get; set; }

            public string Direccion { get; set; }
        }




    
}
