using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Entities
{
    public partial class Shipper
    {
        public Shipper()
        {
            BatchOrders = new HashSet<BatchOrder>();
        }

        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string? Phone { get; set; }
        public Guid? WarehouseId { get; set; }
        public string? Location { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Warehouse? Warehouse { get; set; }
        public virtual ICollection<BatchOrder> BatchOrders { get; set; }
    }
}
