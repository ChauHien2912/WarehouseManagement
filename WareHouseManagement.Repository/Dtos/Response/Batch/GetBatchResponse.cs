using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Response.Batch
{
    public class GetBtachResponse
    {
        public Guid Id { get; set; }
        public Guid? WarehouseId { get; set; }
        public string? BatchMode { get; set; }
        public DateTime? DateExported { get; set; }
        public DateTime? DateInported { get; set; }
    }
}
