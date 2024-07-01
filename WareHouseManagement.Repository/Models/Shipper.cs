using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Models
{
    public partial class Shipper
    {
        public Shipper()
        {
            Batches = new HashSet<Batch>();
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
        public virtual ICollection<Batch> Batches { get; set; }
    }
}
