using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Request.WareHouse
{
    public class UpdateBatchMode
    {
        public Guid? warehouseId {  get; set; }

        public Guid? batchId { get; set; }
    }
}
