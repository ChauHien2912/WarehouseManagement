using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Models
{
    public partial class RefreshTokenAccount
    {
        public Guid Id { get; set; }
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime Expires { get; set; }
        public Guid AccountId { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
