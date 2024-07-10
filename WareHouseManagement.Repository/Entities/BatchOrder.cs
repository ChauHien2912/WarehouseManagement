using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Entities
{
    public partial class BatchOrder
    {
        public Guid Id { get; set; }
        public Guid? BatchId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ShipperId { get; set; }
        public string? Img { get; set; }
        public string? Status { get; set; }

        public virtual Batch? Batch { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Shipper? Shipper { get; set; }
    }
}
