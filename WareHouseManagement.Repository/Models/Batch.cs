using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Models
{
    public partial class Batch
    {
        public Batch()
        {
            BatchOrders = new HashSet<BatchOrder>();
        }

        public Guid Id { get; set; }
        public Guid? ShipperId { get; set; }
        public Guid? WarehouseId { get; set; }
        public string? BatchMode { get; set; }
        public string? Img { get; set; }
        public DateTime? DateModifiedBatchMode { get; set; }

        public virtual Shipper? Shipper { get; set; }
        public virtual Warehouse? Warehouse { get; set; }
        public virtual ICollection<BatchOrder> BatchOrders { get; set; }
    }
}
