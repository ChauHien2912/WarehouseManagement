using System;
using System.Collections.Generic;

namespace WareHouseManagement.Repository.Entities
{
    public partial class Account
    {
        public Account()
        {
            Admins = new HashSet<Admin>();
            RefreshTokenAccounts = new HashSet<RefreshTokenAccount>();
            Shippers = new HashSet<Shipper>();
            Warehouses = new HashSet<Warehouse>();
        }

        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Guid? RoleId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<RefreshTokenAccount> RefreshTokenAccounts { get; set; }
        public virtual ICollection<Shipper> Shippers { get; set; }
        public virtual ICollection<Warehouse> Warehouses { get; set; }
    }
}
