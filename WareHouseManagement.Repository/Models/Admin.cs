using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Models
{
    public partial class Admin
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public string? BatchMode { get; set; }

        public virtual Account? Account { get; set; }
    }
}
