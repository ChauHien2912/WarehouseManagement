using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.NewFolder;
using WareHouseManagement.Repository.Dtos.Request.Shippper;
using WareHouseManagement.Repository.Dtos.Response.Order;
using WareHouseManagement.Repository.Dtos.Response.Shipper;
using WareHouseManagement.Repository.Specifications;

namespace WareHouseManagement.Repository.Services.IServices
{
    public interface IShipperService
    {
        Task<IPaginate<GetShipperResponse>> GetShippers(Guid warehouseid, int page, int size);

        Task<GetShipperResponse> GetShipperById(Guid id);

        Task<bool> UpdateShipper(Guid id, UpdateShipperRequest request);

        Task<bool> DeleteShipperById(Guid id);

        Task<IPaginate<GetOrderResponse>> GetOrderOfShipperByImported(Guid id, string Batchmode ,int page, int size);

        
    }
}
