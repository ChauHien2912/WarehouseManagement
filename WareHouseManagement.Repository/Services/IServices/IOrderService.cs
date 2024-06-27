using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Order;
using WareHouseManagement.Repository.Dtos.Response.Order;
using WareHouseManagement.Repository.Specifications;

namespace WareHouseManagement.Repository.Services.IServices
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(CreateOrderRequest request);

        Task<IPaginate<GetOrderResponse>> GetOrders(int page, int size);

        Task<GetOrderResponse> GetOrderById(Guid id);

        Task<int> UpdataBatchModeByWarehouse(Guid warehouseid);

        Task<IPaginate<GetOrderResponse>> GetOrderByShipper(Guid shipperid, int page, int size);

        
    }
}
