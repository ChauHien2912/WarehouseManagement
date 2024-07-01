using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Models
{
    public partial class Warehouse
    {
        public Warehouse()
        {
            Batches = new HashSet<Batch>();
            Orders = new HashSet<Order>();
            Shippers = new HashSet<Shipper>();
        }

        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ICollection<Batch> Batches { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Shipper> Shippers { get; set; }
    }
}
