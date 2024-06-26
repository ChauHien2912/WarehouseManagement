﻿using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Models
{
    public partial class BatchOrder
    {
        public Guid Id { get; set; }
        public Guid? BatchId { get; set; }
        public Guid? OrderId { get; set; }

        public virtual Batch? Batch { get; set; }
        public virtual Order? Order { get; set; }
    }
}
