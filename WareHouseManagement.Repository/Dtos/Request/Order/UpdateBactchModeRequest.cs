using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Request.Order
{
    public class UpdateBactchModeRequest
    {
        public IFormFile? image {  get; set; }

        public Guid? BatchOrderId {  get; set; }

        public string Status { get; set; }
    }

    
}
