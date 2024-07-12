using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Order;
using WareHouseManagement.Repository.Dtos.Response.Order;
using WareHouseManagement.Repository.Enum;
using WareHouseManagement.Repository.Entities;
using WareHouseManagement.Repository.Repository;
using WareHouseManagement.Repository.Services.IServices;
using WareHouseManagement.Repository.Specifications;
using WareHouseManagement.Repository.Dtos.Request.WareHouse;
using WareHouseManagement.Repository.Dtos.Response.Batch;

namespace WareHouseManagement.Repository.Services.Services
{
    public class OrderService : IOrderService
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;

        public OrderService(IUnitOfWork uow, IMapper mapper, IMediaService mediaService)
        {
            _uow = uow;
            _mapper = mapper;
            _mediaService = mediaService;
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
                                Id = Guid.NewGuid(),
                                OrderDate = worksheet.Cells[row, 1].GetValue<DateTime>(),
                                ExpectedDateOfDelivery = worksheet.Cells[row, 2].GetValue<DateTime>(),
                                Price = worksheet.Cells[row, 3].GetValue<decimal>(),
                                DeliveryDate = worksheet.Cells[row, 4].GetValue<DateTime>(),
                                Img = worksheet.Cells[row, 5].GetValue<string>(),
                                Address = worksheet.Cells[row, 6].GetValue<string>(),
                                Name = worksheet.Cells[row,7].GetValue<string>(),
                                Phone = worksheet.Cells[row, 8].GetValue<string>(),
                                WarehouseId = request.WarehouseId
                                
                            };
                            orders.Add(order);
                        }
                    }
                }

                var shippers = await _uow.GetRepository<Shipper>().GetListAsync(predicate: s => s.WarehouseId == request.WarehouseId);
                var shipperList = shippers.ToList();
                if (!shipperList.Any())
                {
                    return false;
                }
                var batch = new Batch
                {
                    Id = Guid.NewGuid(),
                    WarehouseId = request.WarehouseId,
                    BatchMode = BatchMode.TRUCKIN,
                    DateExported = DateTime.Now,
                };
                batches.Add(batch);
                int shipperIndex = 0;

                foreach (var order in orders)
                {

                    var shipper = shipperList[shipperIndex];

                    // Create a new batch order
                    var batchOrder = new BatchOrder
                    {
                        Id = Guid.NewGuid(),
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

        public async Task<IPaginate<GetBtachResponse>> GetBatchByWarehouseId(Guid id, string batchmode, int page, int size)
        {
            var batchs = await _uow.GetRepository<Batch>().GetPagingListAsync(predicate: p => p.WarehouseId == id && p.BatchMode == batchmode,
                        page: page, size: size, orderBy: o => o.OrderBy( p => p.DateInported)
                        );
            var batchResponses = new Paginate<GetBtachResponse>()
            {
                Page = batchs.Page,
                Size = batchs.Size,
                Total = batchs.Total,
                TotalPages = batchs.TotalPages,
                Items = _mapper.Map<IList<GetBtachResponse>>(batchs.Items),
            };

            return batchResponses;
        }

        public async Task<IPaginate<GetOrderResponse>> GetListOrderByBatchId(Guid id, int page, int size)
        {
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.BatchId == id,
                        page: page, size:size, include: i => i.Include( m => m.Order)
                );
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

        public async Task<GetOrderResponse> GetOrderById(Guid id)
        {
            var order = await _uow.GetRepository<Order>().SingleOrDefaultAsync(predicate: o => o.Id == id);
            if (order == null)
            {
                throw new Exception("Order Not Found");
            }
            return _mapper.Map<GetOrderResponse>(order);

        }

        public async Task<IPaginate<GetOrderResponse>> GetOrderByShipper(Guid shipperid, int page, int size)
        {
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(
        predicate: p => p.ShipperId == shipperid,
        page: page,
        size: size,
        include: i => i.Include(bo => bo.Order).Include(bo => bo.Batch),
        orderBy: o => o.OrderBy(o => o.Order.OrderDate)
    );

            // Map the results to GetOrderResponse
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

        public async Task<IPaginate<GetOrderResponse>> GetOrderDeliveing(int page, int size)
        {
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.Status == BatchMode.DELIVERING,
                                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                                orderBy: i => i.OrderBy(i => i.Order.DeliveryDate)
                                );
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

        public async Task<IPaginate<GetOrderResponse>> GetOrderFail(int page, int size)
        {
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.Status == BatchMode.FAIL,
                                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                                orderBy: i => i.OrderBy(i => i.Order.DeliveryDate)
                                );
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

        public async Task<IPaginate<GetOrderResponse>> GetOrderImported(int page, int size)
        {
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.Batch.BatchMode == BatchMode.IMPORTED,
                                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                                orderBy: i => i.OrderBy(i => i.Order.DeliveryDate)
                                );
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

        public async Task<IPaginate<GetOrderResponse>> GetOrderOfWarehouseByBatchMode(Guid warehouseid, string BatchMode, int page, int size)
        {
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.Batch.WarehouseId == warehouseid 
                        && p.Batch.BatchMode == BatchMode, 
                        include: i => i.Include(d => d.Order),
                        orderBy: o => o.OrderBy(m => m.Order.ImportedDate)
                        );
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

        public async Task<IPaginate<GetOrderResponse>> GetOrders(int page, int size)
        {
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(page : page, size: size,
                                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                                orderBy: i => i.OrderBy(i => i.Order.DeliveryDate)
                                );

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

        public async Task<IPaginate<GetOrderResponse>> GetOrderSuccess(int page, int size)
        {
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.Status == BatchMode.SUCCESS,
                                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                                orderBy: i => i.OrderBy(i => i.Order.DeliveryDate)
                                );
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

        public async Task<IPaginate<GetOrderResponse>> GetOrderTruckIn(int page, int size)
        {
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.Batch.BatchMode == BatchMode.TRUCKIN,
                                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                                orderBy: i => i.OrderBy(i => i.Order.DeliveryDate)
                                );
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

        public async Task<IPaginate<GetOrderResponse>> GetOrderTruckOut(int page, int size)
        {
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.Batch.BatchMode == BatchMode.TRUCKOUT,
                                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                                orderBy: i => i.OrderBy(i => i.Order.DeliveryDate)
                                );
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

        public async Task<int> UpdataBatchModeByWarehouse(UpdateBatchMode request)
        {
            // Lấy danh sách shipper trong kho
            // Lấy danh sách shipper trong warehouse
            var shippers = await _uow.GetRepository<Shipper>().GetListAsync(predicate: s => s.WarehouseId == request.warehouseId);
            var shipperList = shippers.ToList();
            if (!shipperList.Any())
            {
                return 0; // Nếu không có shipper nào trong kho, không làm gì cả
            }

            // Lấy danh sách BatchOrder và Order dựa trên batchId
            var batchOrders = await _uow.GetRepository<BatchOrder>().GetListAsync(
                predicate: bo => bo.BatchId == request.batchId,
                include: i => i.Include(bo => bo.Order)
            );

            var batch = await _uow.GetRepository<Batch>().SingleOrDefaultAsync(predicate: b => b.Id == request.batchId);
            if (batch != null)
            {
                batch.BatchMode = BatchMode.IMPORTED;
                batch.DateInported = DateTime.Now;

                int shipperCount = shipperList.Count;
                int shipperIndex = 0;

                foreach (var batchOrder in batchOrders)
                {
                    // Phân chia Order cho các Shipper
                    var currentShipper = shipperList[shipperIndex];
                    batchOrder.ShipperId = currentShipper.Id;
                    batchOrder.Status = BatchMode.DELIVERING; // Đây là một giả định, bạn có thể cần áp dụng logic xử lý trạng thái phù hợp
                    batchOrder.Order.ImportedDate = DateTime.Now; // Cập nhật ImportedDate cho Order

                    // Cập nhật Order
                    _uow.GetRepository<Order>().UpdateAsync(batchOrder.Order);

                    // Di chuyển sang shipper tiếp theo
                    shipperIndex = (shipperIndex + 1) % shipperCount;
                }

                // Cập nhật Batch và BatchOrder
                _uow.GetRepository<Batch>().UpdateAsync(batch);
                _uow.GetRepository<BatchOrder>().UpdateRange(batchOrders);

                // Lưu các thay đổi vào cơ sở dữ liệu
                await _uow.CommitAsync();

                return batchOrders.Count;
            }
            else
            {
                return 0; // Nếu không tìm thấy Batch với Id tương ứng
            }

        }




        public async Task<bool> UpdateBatchModeByShipper(UpdateBactchModeRequest request)
        {

            var batchorder = await _uow.GetRepository<BatchOrder>().SingleOrDefaultAsync(
                predicate: p => p.BatchId == request.BatchId);
            var batch = await _uow.GetRepository<Batch>().SingleOrDefaultAsync(
                        predicate: p => p.Id == batchorder.BatchId
                        );

            if (request.image == null || request.image.Length == 0)
            {
                throw new Exception("You need to add Image for updating success order");

            }
            var urlImg = await _mediaService.UploadAnImage(request.image, MediaPath.BATCHMODE_IMG, batch.Id.ToString());

            StaticFile staticFile = new StaticFile()
            {
                
                Id = Guid.NewGuid(),
                OrderId = (Guid)batchorder.OrderId,
                Img = urlImg,
            };


            batchorder.Img = urlImg;
            batchorder.Status = request.Status;

            await _uow.GetRepository<StaticFile>().InsertAsync(staticFile);
            _uow.GetRepository<BatchOrder>().UpdateAsync(batchorder);

            bool isUpdated = await _uow.CommitAsync() > 0;

            return isUpdated;
        }


    }
}
