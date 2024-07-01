using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Models
{
    public partial class Order
    {
        public Order()
        {
            BatchOrders = new HashSet<BatchOrder>();
            StaticFiles = new HashSet<StaticFile>();
        }

        public Guid Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ExpectedDateOfDelivery { get; set; }
        public decimal? Price { get; set; }
        public Guid? WarehouseId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Img { get; set; }
        public string? Address { get; set; }

        public virtual Warehouse? Warehouse { get; set; }
        public virtual ICollection<BatchOrder> BatchOrders { get; set; }
        public virtual ICollection<StaticFile> StaticFiles { get; set; }
    }
}
