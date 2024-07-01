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
using WareHouseManagement.Repository.Models;
using WareHouseManagement.Repository.Repository;
using WareHouseManagement.Repository.Services.IServices;
using WareHouseManagement.Repository.Specifications;

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

                int shipperIndex = 0;

                foreach (var order in orders)
                {

                    var shipper = shipperList[shipperIndex];


                    var batch = new Batch
                    {
                        Id = Guid.NewGuid(),
                        ShipperId = null,
                        WarehouseId = request.WarehouseId,
                        BatchMode = BatchMode.TRUCKIN,
                        DateModifiedBatchMode = DateTime.Now,
                    };
                    batches.Add(batch);

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
        predicate: p => p.Batch.ShipperId == shipperid,
        page: page,
        size: size,
        include: i => i.Include(bo => bo.Order).Include(bo => bo.Batch)
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
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.Batch.BatchMode == BatchMode.DELIVERING,
                                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                                orderBy: i => i.OrderByDescending(i => i.Order.DeliveryDate)
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
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.Batch.BatchMode == BatchMode.FAIL,
                                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                                orderBy: i => i.OrderByDescending(i => i.Order.DeliveryDate)
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
                                orderBy: i => i.OrderByDescending(i => i.Order.DeliveryDate)
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

        public async Task<IPaginate<GetOrderResponse>> GetOrderSuccess(int page, int size)
        {
            var orders = await _uow.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.Batch.BatchMode == BatchMode.SUCCESS,
                                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                                orderBy: i => i.OrderByDescending(i => i.Order.DeliveryDate)
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
                                orderBy: i => i.OrderByDescending(i => i.Order.DeliveryDate)
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
                                orderBy: i => i.OrderByDescending(i => i.Order.DeliveryDate)
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

        public async Task<int> UpdataBatchModeByWarehouse(Guid warehouseid)
        {
            // Lấy danh sách shipper trong kho
            var shippers = await _uow.GetRepository<Shipper>().GetListAsync(predicate: p => p.WarehouseId == warehouseid);
            var shipperList = shippers.ToList();
            if (!shipperList.Any())
            {
                return 0; // Nếu không có shipper nào trong kho, trả về 0
            }

            // Lấy danh sách batch có BatchMode là "truckin" trong kho
            List<Batch> batches = (List<Batch>)await _uow.GetRepository<Batch>().GetListAsync(predicate: b => b.WarehouseId == warehouseid && b.BatchMode == "truckin");

            // Đếm số lượng batch đã cập nhật
            int updatedBatchCount = 0;

            // Phân chia đều các batch cho các shipper
            int shipperIndex = 0;
            foreach (var batch in batches)
            {
                batch.BatchMode = BatchMode.IMPORTED; // Cập nhật trạng thái của batch
                batch.ShipperId = shipperList[shipperIndex].Id; // Gán batch cho shipper
                batch.DateModifiedBatchMode = DateTime.Now;
                shipperIndex = (shipperIndex + 1) % shipperList.Count; // Di chuyển đến shipper tiếp theo
                _uow.GetRepository<Batch>().UpdateAsync(batch);
                updatedBatchCount++; // Tăng biến đếm
            }

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _uow.CommitAsync();

            return updatedBatchCount; // Trả về số lượng batch đã cập nhật
        }

        public async Task<bool> UpdateBatchModeByShipper(UpdateBactchModeRequest request)
        {

            var batchorder = await _uow.GetRepository<BatchOrder>().SingleOrDefaultAsync(
                        predicate: p => p.Id == request.BatchOrderId
                        );
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


            batch.Img = urlImg;
            batch.BatchMode = request.Status;
            batch.DateModifiedBatchMode = DateTime.Now;


            await _uow.GetRepository<StaticFile>().InsertAsync(staticFile);
            _uow.GetRepository<Batch>().UpdateAsync(batch);

            bool isUpdated = await _uow.CommitAsync() > 0;

            return isUpdated;
        }
    }


}
