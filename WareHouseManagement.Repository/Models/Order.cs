using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Models
{
    public partial class Order
    {
        public Order()
        {
            BatchOrders = new HashSet<BatchOrder>();
        }

        public int Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ExpectedDateOfDelivery { get; set; }
        public decimal? Price { get; set; }
        public int? WarehouseId { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public virtual Warehouse? Warehouse { get; set; }
        public virtual ICollection<BatchOrder> BatchOrders { get; set; }
    }
}
