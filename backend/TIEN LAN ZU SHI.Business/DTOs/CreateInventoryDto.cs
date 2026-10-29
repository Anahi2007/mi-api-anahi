using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIEN_LAN_ZU_SHI.Business.DTOs
{
    public class CreateInventoryDto
    {
        public int ProductId { get; set; }
        public DateTime Date { get; set; }
        public int DailyStock { get; set; }
        public decimal SalePrice {  get; set; }
    }
}
