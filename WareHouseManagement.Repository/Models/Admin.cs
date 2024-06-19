using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Models
{
    public partial class Admin
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }

        public virtual Account? Account { get; set; }
    }
}
