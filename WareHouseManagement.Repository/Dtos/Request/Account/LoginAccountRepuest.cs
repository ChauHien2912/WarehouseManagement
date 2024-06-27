using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Request.User
{
    public class LoginAccountRepuest
    {
        public string Email { get; set; } = null!;  
        public string Password { get; set; } = null!;
    }
}
