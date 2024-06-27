using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Entities
{
    public partial class StaticFile
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string? Img { get; set; }
        public string? Video { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
