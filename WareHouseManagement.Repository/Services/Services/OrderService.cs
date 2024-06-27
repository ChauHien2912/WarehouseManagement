using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Order;
using WareHouseManagement.Repository.Dtos.Response.Order;
using WareHouseManagement.Repository.Entities;
using WareHouseManagement.Repository.Repository;
using WareHouseManagement.Repository.Services.IServices;
using WareHouseManagement.Repository.Specifications;

namespace WareHouseManagement.Repository.Services.Services
{
    public class OrderService : IOrderService
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<bool> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                if (request.Orderfile == null || request.Orderfile.Length == 0)
                {
                    return false;
                }

                var orders = new List<Order>();
                var batches = new List<Batch>();
                var batchOrders = new List<BatchOrder>();
                using (var stream = new MemoryStream())
                {
                    await request.Orderfile.CopyToAsync(stream);
                    // Thiết lập LicenseContext
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (var row = 2; row <= rowCount; row++) // Bắt đầu từ hàng 2 để bỏ qua tiêu đề
                        {
                            var order = new Order
                            {
                                OrderDate = worksheet.Cells[row, 1].GetValue<DateTime>(),
                                ExpectedDateOfDelivery = worksheet.Cells[row, 2].GetValue<DateTime>(),
                                Price = worksheet.Cells[row, 3].GetValue<decimal>(),
                                DeliveryDate = worksheet.Cells[row, 4].GetValue<DateTime>(),
                                Img = worksheet.Cells[row, 5].GetValue<string>(),
                                WarehouseId = request.WarehouseId
                            };
                            orders.Add(order);
                        }
                    }
                }

                var shippers = await _uow.GetRepository<Shipper>().GetListAsync(predicate: s => s.WarehouseId == request.WarehouseId);
                var shipperList = shippers.ToList(); // Chuyển đổi sang List<Shipper>
                if (!shipperList.Any())
                {
                    return false; // Không có shipper nào trong warehouse
                }

                int shipperIndex = 0; // Biến chỉ mục để theo dõi shipper hiện tại

                foreach (var order in orders)
                {
                    // Chọn shipper theo chỉ mục
                    var shipper = shipperList[shipperIndex];

                    // Create a new batch
                    var batch = new Batch
                    {
                        ShipperId = shipper.Id,
                        WarehouseId = request.WarehouseId,
                        BatchMode = "truckin"
                    };
                    batches.Add(batch);

                    // Create a new batch order
                    var batchOrder = new BatchOrder
                    {
                        OrderId = order.Id,
                        BatchId = batch.Id
                    };
                    batchOrders.Add(batchOrder);

                    // Chuyển sang shipper tiếp theo theo vòng lặp
                    shipperIndex = (shipperIndex + 1) % shipperList.Count;
                }

                await _uow.GetRepository<Order>().InsertRangeAsync(orders);
                await _uow.GetRepository<Batch>().InsertRangeAsync(batches);
                await _uow.GetRepository<BatchOrder>().InsertRangeAsync(batchOrders);

                bool isCreated = await _uow.CommitAsync() > 0;
                return isCreated;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<GetOrderResponse> GetOrderById(Guid id)
        {
            var order = await _uow.GetRepository<Order>().SingleOrDefaultAsync(predicate: o => o.Id == id);
            if(order == null)
            {
                throw new Exception("Order Not Found");
            }
            return _mapper.Map<GetOrderResponse>(order);

        }

        public async Task<IPaginate<GetOrderResponse>> GetOrders(int page, int size)
        {
            var orders = await _uow.GetRepository<Order>().GetPagingListAsync(page: page, size: size);

            var orderResponses = new Paginate<GetOrderResponse>()
            {
                Page = orders.Page,
                Size = orders.Size,
                Total = orders.Total,
                TotalPages = orders.TotalPages,
                Items = _mapper.Map<IList<GetOrderResponse>>(orders.Items),
            };
            return orderResponses;

        }
    }
}
