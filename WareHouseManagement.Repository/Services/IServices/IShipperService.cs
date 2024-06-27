using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Response.Shipper;
using WareHouseManagement.Repository.Specifications;

namespace WareHouseManagement.Repository.Services.IServices
{
    public interface IShipperService
    {
        Task<IPaginate<GetShipperResponse>> GetShippers(Guid warehouseid, int page, int size);

        Task<GetShipperResponse> GetShipperById(Guid id);
    }
}
