using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Response.WareHouse
{
    public class GetWarehouseResponse
    {
        public GetWarehouseResponse() { }

        public GetWarehouseResponse(Guid id, int? accountId, string? name, string? phone, string? location)
        {
            Id = id;
            AccountId = accountId;
            Name = name;
            Phone = phone;
            Location = location;
        }

        public Guid Id { get; set; }
        public int? AccountId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
    }
}
