using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Models
{
    public partial class Truck
    {
        public Truck()
        {
            Batches = new HashSet<Batch>();
        }

        public int Id { get; set; }
        public string? LicensePlates { get; set; }

        public virtual ICollection<Batch> Batches { get; set; }
    }
}
