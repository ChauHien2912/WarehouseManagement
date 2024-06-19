using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WareHouseManagement.Repository.Dtos.Response.Authen
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int AccountId { get; set; }
        
        public string RoleName { get; set; }

        public ShipperLoginResponse ShipperResponse { get; set; }
        public WarehouseLoginResponse WarehouseResponse { get; set; }

    }

    public class ShipperLoginResponse
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string? Phone { get; set; }
        public int? WarehouseId { get; set; }
    }

    public class WarehouseLoginResponse
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
    }
}
