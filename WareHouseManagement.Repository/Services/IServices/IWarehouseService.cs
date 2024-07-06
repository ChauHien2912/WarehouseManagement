using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.WareHouse;
using WareHouseManagement.Repository.Dtos.Response.Order;
using WareHouseManagement.Repository.Dtos.Response.Shipper;
using WareHouseManagement.Repository.Dtos.Response.WareHouse;
using WareHouseManagement.Repository.Specifications;

namespace WareHouseManagement.Repository.Services.IServices
{
    public interface IWarehouseService
    {
        Task<IPaginate<GetWarehouseResponse>> GetWarehouses(int page, int size);
        Task<GetWarehouseResponse>? GetWarehouseById(Guid id);
        Task<bool> UpdateWarehouseById(Guid id, UpdateWarehouseRequest updateWarehouseRequest);
        Task<bool> DeleteWarehouseById(Guid id);
        
        Task<IPaginate<GetOrderResponse>> GetOrderSuccessByWarehouse(Guid warehouseid, int page, int size);

        Task<IPaginate<GetOrderResponse>> GetOrderFailByWarehouse(Guid warehousebyid, int page, int size);

        

        Task<decimal> GetRevenueByImprorted(Guid warehouseid, int day, int month, int year);

        Task<decimal> GetRevenueBySuccess(Guid warehouseid, int day, int month, int year);

        Task<IPaginate<GetShipperResponse>> GetShipperByWarehouse(Guid id, int page, int size);

        Task<int> GetShippers(Guid id);

        Task<bool> ExportFileExcelOrderFail(Guid id);

        Task<int> GetOrderImportedInDate(Guid id, int day, int month, int year);

        Task<int> GetOrderExportedInDate(Guid id, int day, int month, int year);

        Task<decimal> GetRateSuccessOfOrder(Guid id);

        Task<decimal> GetRateFailOfOrder(Guid id);

        Task<int> GetTotalOrdersByWarehouse(Guid warehouseId);
    }
}
