using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Order;
using WareHouseManagement.Repository.Dtos.Request.WareHouse;
using WareHouseManagement.Repository.Dtos.Response.Batch;
using WareHouseManagement.Repository.Dtos.Response.Order;
using WareHouseManagement.Repository.Specifications;

namespace WareHouseManagement.Repository.Services.IServices
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(CreateOrderRequest request);

        Task<IPaginate<GetOrderResponse>> GetOrders(int page, int size);

        Task<GetOrderResponse> GetOrderById(Guid id);

        Task<int> UpdataBatchModeByWarehouse(UpdateBatchMode reqest);

        Task<IPaginate<GetOrderResponse>> GetOrderByShipper(Guid shipperid, int page, int size);

        Task<bool> UpdateBatchModeByShipper(UpdateBactchModeRequest request);

        Task<IPaginate<GetOrderResponse>> GetOrderTruckOut(int page, int size);
        Task<IPaginate<GetOrderResponse>> GetOrderImported(int page, int size);
        Task<IPaginate<GetOrderResponse>> GetOrderDeliveing(int page, int size);
        Task<IPaginate<GetOrderResponse>> GetOrderSuccess(int page, int size);
        Task<IPaginate<GetOrderResponse>> GetOrderFail(int page, int size);
        Task<IPaginate<GetOrderResponse>> GetOrderTruckIn(int page, int size);

        Task<IPaginate<GetOrderResponse>> GetOrderOfWarehouseByBatchMode(Guid warehouseid, string BatchMode, int page, int size);

        Task<IPaginate<GetOrderResponse>> GetListOrderByBatchId(Guid id, int page, int size);

        Task<IPaginate<GetBtachResponse>> GetBatchByWarehouseId(Guid id, int page, int size);

    }
}
