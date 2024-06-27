using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Response.Authen
{
    public class FetchUserResponse
    {
        public int AccountId { get; set; }
        public string RoleName { get; set; }
        public ShipperLoginResponse? ShipperResponse { get; set; }
        public WarehouseLoginResponse? WarehouseResponse { get; set; }
    }
}
