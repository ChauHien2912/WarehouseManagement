using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Request.Order
{
    public class CreateOrderRequest
    {
        public Guid WarehouseId { get; set; }    
        public IFormFile? Orderfile { get; set; }
    }
}
