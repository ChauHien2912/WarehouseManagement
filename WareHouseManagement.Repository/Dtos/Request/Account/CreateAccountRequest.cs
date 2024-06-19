using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Request.Account
{
    public class CreateAccountRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? RoleName { get; set; }
    }
}
