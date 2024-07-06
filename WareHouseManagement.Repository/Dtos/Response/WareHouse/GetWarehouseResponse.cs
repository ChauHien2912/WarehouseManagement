using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Response.WareHouse
{
    public class GetWarehouseResponse
    {

        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
    }
}
