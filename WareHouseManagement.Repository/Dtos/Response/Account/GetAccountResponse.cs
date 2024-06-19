using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WareHouseManagement.Repository.Dtos.Response.User
{
    public class GetAccountResponse
    {
        public GetAccountResponse() { }

        public GetAccountResponse(int id, string? email, string? password, int? roleId, bool? isActive)
        {
            Id = id;
            Email = email;
            Password = password;
            RoleId = roleId;
            IsActive = isActive;
        }

        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }

    }
}
