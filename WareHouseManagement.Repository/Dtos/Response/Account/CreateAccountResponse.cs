using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Response.Account
{
    public class CreateAccountResponse
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Guid? RoleId { get; set; }
    }
}
