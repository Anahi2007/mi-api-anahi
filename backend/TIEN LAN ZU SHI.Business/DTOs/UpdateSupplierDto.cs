using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIEN_LAN_ZU_SHI.Business.DTOs
{
    public class UpdateSupplierDto
    {
        public int Id { get; set; } // Para identificar cuál actualizar
        public string NombreProveedor { get; set; }
        public string Contacto { get; set; }
        public string Ciudad { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }

    }
}
