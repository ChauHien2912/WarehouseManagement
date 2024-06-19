using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Models
{
    public partial class BatchOrder
    {
        public int Id { get; set; }
        public int? BatchId { get; set; }
        public int? OrderId { get; set; }

        public virtual Batch? Batch { get; set; }
        public virtual Order? Order { get; set; }
    }
}
