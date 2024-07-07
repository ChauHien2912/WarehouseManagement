using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WareHouseManagement.Repository.Dtos.Response.User
{
    public class GetAccountResponse
    {
       

        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? RoleName { get; set; }
        public bool? IsActive { get; set; }

        public string? FullName { get; set; }

    }
}
