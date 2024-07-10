using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Entities
{
    public partial class Batch
    {
        public Batch()
        {
            BatchOrders = new HashSet<BatchOrder>();
        }

        public Guid Id { get; set; }
        public Guid? WarehouseId { get; set; }
        public string? BatchMode { get; set; }
        public DateTime? DateExported { get; set; }
        public DateTime? DateInported { get; set; }

        public virtual Warehouse? Warehouse { get; set; }
        public virtual ICollection<BatchOrder> BatchOrders { get; set; }
    }
}
