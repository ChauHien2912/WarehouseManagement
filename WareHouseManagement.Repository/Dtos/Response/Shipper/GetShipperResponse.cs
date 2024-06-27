using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Response.Shipper
{
    public class GetShipperResponse
    {
        public GetShipperResponse() { }

        public GetShipperResponse(Guid id, int? accountId, string? fullName, DateTime? dateOfBirth, bool? gender, string? phone, Guid? warehouseId)
        {
            Id = id;
            AccountId = accountId;
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Phone = phone;
            WarehouseId = warehouseId;
        }

        public Guid Id { get; set; }
        public int? AccountId { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string? Phone { get; set; }
        public Guid? WarehouseId { get; set; }
    }
}
