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

        public int Id { get; set; }
        public string? BatchMode { get; set; }
        public int? ShipperId { get; set; }
        public int? WarehouseId { get; set; }
        public int? TruckId { get; set; }

        public virtual Shipper? Shipper { get; set; }
        public virtual Truck? Truck { get; set; }
        public virtual Warehouse? Warehouse { get; set; }
        public virtual ICollection<BatchOrder> BatchOrders { get; set; }
    }
}
