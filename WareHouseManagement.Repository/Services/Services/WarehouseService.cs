using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.WareHouse;
using WareHouseManagement.Repository.Dtos.Response.Order;
using WareHouseManagement.Repository.Dtos.Response.Shipper;
using WareHouseManagement.Repository.Dtos.Response.WareHouse;
using WareHouseManagement.Repository.Enum;
using WareHouseManagement.Repository.Entities;

//using WareHouseManagement.Repository.Models;
using WareHouseManagement.Repository.Repository;
using WareHouseManagement.Repository.Services.IServices;
using WareHouseManagement.Repository.Specifications;
using ClosedXML.Excel;

namespace WareHouseManagement.Repository.Services.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public WarehouseService(IUnitOfWork uof, IConfiguration config, IMapper mapper)
        {
            _uof = uof;
            _config = config;
            _mapper = mapper;
        }

        

        public async Task<bool> DeleteWarehouseById(Guid id)
        {
            var existing = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: e => e.Id == id);
            if (existing == null)
            {
                throw new Exception("Cannot Find Warehouse");
            }
            var accountid = await _uof.GetRepository<Account>().SingleOrDefaultAsync(predicate: e => e.Id == existing.AccountId);
            _uof.GetRepository<Warehouse>().DeleteAsync(existing);
            _uof.GetRepository<Account>().DeleteAsync(accountid);
            bool isDeleted = await _uof.CommitAsync() > 0;
            return isDeleted;
        }

        public async Task<bool> ExportFileExcelOrderFail(Guid id)
        {
            var warehouse = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: p => p.Id == id);
            if (warehouse == null)
            {
                throw new Exception("Not Found Warehouse");
            }
            var orders = await _uof.GetRepository<BatchOrder>().GetListAsync(predicate: p => p.Batch.WarehouseId == id && p.Batch.BatchMode == BatchMode.FAIL,
                                orderBy: o => o.OrderBy(d => d.Order.ImportedDate),
                               include: i => i.Include(id => id.Order)
                                );
            var orderList = orders.Select(o => new
            {
                o.Order.Id,
                o.Order.OrderDate,
                o.Order.ExpectedDateOfDelivery,
                o.Order.Price,
                o.Order.WarehouseId,
                o.Order.DeliveryDate,
                o.Order.Img,
                o.Order.Address,
                o.Order.ImportedDate,
                o.Order.ExportedDate
            }).ToList();
            DateTime exportedDate = DateTime.Now;

            // Update ExportedDate in each order
            foreach (var order in orders)
            {
                order.Batch.BatchMode = BatchMode.TRUCKOUT;
                order.Order.ExportedDate = exportedDate;
                _uof.GetRepository<Order>().UpdateAsync(order.Order);
                _uof.GetRepository<Batch>().UpdateAsync(order.Batch);
            }
            await _uof.CommitAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Orders");
                var currentRow = 1;

                // Header
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "OrderDate";
                worksheet.Cell(currentRow, 3).Value = "ExpectedDateOfDelivery";
                worksheet.Cell(currentRow, 4).Value = "Price";
                worksheet.Cell(currentRow, 5).Value = "WarehouseId";
                worksheet.Cell(currentRow, 6).Value = "DeliveryDate";
                worksheet.Cell(currentRow, 7).Value = "Img";
                worksheet.Cell(currentRow, 8).Value = "Address";
                worksheet.Cell(currentRow, 9).Value = "ImportedDate";
                worksheet.Cell(currentRow, 10).Value = "ExportedDate";

                // Content
                foreach (var order in orderList)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = order.Id.ToString();
                    worksheet.Cell(currentRow, 2).Value = order.OrderDate ?? DateTime.MinValue;
                    worksheet.Cell(currentRow, 3).Value = order.ExpectedDateOfDelivery ?? DateTime.MinValue;
                    worksheet.Cell(currentRow, 4).Value = order.Price ?? 0;
                    worksheet.Cell(currentRow, 5).Value = order.WarehouseId.ToString();
                    worksheet.Cell(currentRow, 6).Value = order.DeliveryDate ?? DateTime.MinValue;
                    worksheet.Cell(currentRow, 7).Value = order.Img ?? "";
                    worksheet.Cell(currentRow, 8).Value = order.Address ?? "";
                    worksheet.Cell(currentRow, 9).Value = order.ImportedDate ?? DateTime.MinValue;
                    worksheet.Cell(currentRow, 10).Value = exportedDate; // Use the updated exportedDate
                }

                var filePath = $"Orders_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                workbook.SaveAs(filePath);

                // Return true to indicate the file was created successfully
                return true;

            }
        }

        public async Task<int> GetOrderExportedInDate(Guid id, int day, int month, int year)
        {
            var warehouse = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: p => p.Id == id);
            if (warehouse == null)
            {
                throw new Exception("Not Found Warehouse");
            }

            var orders = await _uof.GetRepository<BatchOrder>().GetListAsync(
                predicate: p => p.Batch.BatchMode == BatchMode.TRUCKOUT
                                && p.Batch.WarehouseId == id
                                && p.Order.ExportedDate.Value.Month == month
                                && p.Order.ExportedDate.Value.Year == year && p.Order.ExportedDate.Value.Day == day,
                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                orderBy: i => i.OrderBy(i => i.Order.ExportedDate)
            );

            return orders.Count();
        }

        public async Task<IPaginate<GetOrderResponse>> GetOrderFailByWarehouse(Guid warehousebyid, int page, int size)
        {
            var warehouse = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: p => p.Id == warehousebyid);
            if(warehouse == null)
            {
                throw new Exception("Not Found Warehouse");
            }
            var orders = await _uof.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.Batch.BatchMode == BatchMode.FAIL && p.Batch.WarehouseId == warehousebyid,
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

        public async Task<int> GetOrderImportedInDate(Guid id, int day, int month, int year)
        {
            var warehouse = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: p => p.Id == id);
            if (warehouse == null)
            {
                throw new Exception("Not Found Warehouse");
            }

            var orders = await _uof.GetRepository<BatchOrder>().GetListAsync(
                predicate: p => p.Batch.BatchMode == BatchMode.IMPORTED
                                && p.Batch.WarehouseId == id
                                && p.Order.ExportedDate.Value.Month == month
                                && p.Order.ExportedDate.Value.Year == year && p.Order.ExportedDate.Value.Day == day,
                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                orderBy: i => i.OrderBy(i => i.Order.ExportedDate)
            );

            return orders.Count();
        }

        public async Task<IPaginate<GetOrderResponse>> GetOrderSuccessByWarehouse(Guid warehouseid, int page, int size)
        {
            var warehouse = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: p => p.Id == warehouseid);
            if (warehouse == null)
            {
                throw new Exception("Not Found Warehouse");
            }
            var orders = await _uof.GetRepository<BatchOrder>().GetPagingListAsync(predicate: p => p.Batch.BatchMode == BatchMode.SUCCESS && p.Batch.WarehouseId == warehouseid,
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

        public async Task<int> GetTotalOrdersByWarehouse(Guid warehouseId)
        {
            var warehouse = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: p => p.Id == warehouseId);
            if (warehouse == null)
            {
                throw new Exception("Not Found Warehouse");
            }
            var totalOrders = await _uof.GetRepository<BatchOrder>().GetListAsync(predicate: p => p.Batch.WarehouseId == warehouseId);
            return totalOrders.Count();
        }

        public async Task<decimal> GetRateFailOfOrder(Guid id)
        {
            var orders = await _uof.GetRepository<BatchOrder>().GetListAsync(predicate: p => p.Batch.BatchMode == BatchMode.FAIL && p.Batch.WarehouseId == id,
                                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                                orderBy: i => i.OrderBy(i => i.Order.DeliveryDate)
                                );

            int failedOrdersCount = orders.Count();

            var totalOrders = await GetTotalOrdersByWarehouse(id);

            if (totalOrders == 0)
            {
                return 0;
            }

            return (decimal)failedOrdersCount / totalOrders;
        }

        public async  Task<decimal> GetRateSuccessOfOrder(Guid id)
        {
            var orders = await _uof.GetRepository<BatchOrder>().GetListAsync(predicate: p => p.Batch.BatchMode == BatchMode.SUCCESS && p.Batch.WarehouseId == id,
                                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                                orderBy: i => i.OrderBy(i => i.Order.DeliveryDate)
                                );

            int successOrdersCount = orders.Count();

            var totalOrders = await GetTotalOrdersByWarehouse(id);

            if (totalOrders == 0)
            {
                return 0;
            }

            return (decimal)successOrdersCount / totalOrders;
        }

        public async Task<decimal> GetRevenueByImprorted(Guid warehouseid, int day, int month, int year)
        {
            var warehouse = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: p => p.Id == warehouseid);
            if (warehouse == null)
            {
                throw new Exception("Not Found Warehouse");
            }

            var orders = await _uof.GetRepository<BatchOrder>().GetListAsync(
                predicate: p => p.Batch.BatchMode == BatchMode.IMPORTED
                                && p.Batch.WarehouseId == warehouseid
                                && p.Order.DeliveryDate.Value.Month == month
                                && p.Order.DeliveryDate.Value.Year == year && p.Order.DeliveryDate.Value.Day == day,
                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                orderBy: i => i.OrderBy(i => i.Order.DeliveryDate)
            );

            // Calculate the total price directly from the orders
            decimal totalPrice = (decimal)orders.Sum(order => order.Order.Price);

            return totalPrice;
        }

        public async Task<decimal> GetRevenueBySuccess(Guid warehouseid,int day, int month, int year)
        {
            var warehouse = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: p => p.Id == warehouseid);
            if (warehouse == null)
            {
                throw new Exception("Not Found Warehouse");
            }

            var orders = await _uof.GetRepository<BatchOrder>().GetListAsync(
                predicate: p => p.Batch.BatchMode == BatchMode.SUCCESS
                                && p.Batch.WarehouseId == warehouseid
                                && p.Order.DeliveryDate.Value.Month == month
                                && p.Order.DeliveryDate.Value.Year == year && p.Order.DeliveryDate.Value.Day == day,
                include: i => i.Include(q => q.Order).Include(m => m.Batch),
                orderBy: i => i.OrderBy(i => i.Order.DeliveryDate)
            );

            // Calculate the total price directly from the orders
            decimal totalPrice = (decimal)orders.Sum(order => order.Order.Price);

            return totalPrice;
        }

        public async Task<IPaginate<GetShipperResponse>> GetShipperByWarehouse(Guid id, int page, int size)
        {
            var shippers = await _uof.GetRepository<Shipper>().GetPagingListAsync(predicate: p => p.WarehouseId == id);
            if (shippers == null)
            {
                throw new Exception("Not Found Shippers");
            }
            var paginateResponse = new Paginate<GetShipperResponse>
            {
                Page = shippers.Page,
                Size = shippers.Size,
                Total = shippers.Total,
                TotalPages = shippers.TotalPages,
                Items = _mapper.Map<IList<GetShipperResponse>>(shippers.Items)
            };
            return paginateResponse;
        }

        public async Task<int> GetShippers(Guid id)
        {
            var warehouse = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: p => p.Id == id);
            if (warehouse == null)
            {
                throw new Exception("Not Found Warehouse");
            }

            var shippers = await _uof.GetRepository<Shipper>().GetListAsync(predicate: s => s.WarehouseId == warehouse.Id);
            int shipperCount = shippers.Count();
            return shipperCount;
        }

        public async Task<GetWarehouseResponse>? GetWarehouseById(Guid id)
        {
            try
            {
                var existing = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: e => e.Id == id);
                if (existing == null)
                {
                    return null;
                }
                return _mapper.Map<GetWarehouseResponse>(existing);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IPaginate<GetWarehouseResponse>> GetWarehouses(int page, int size)
        {
            var warehouses = await _uof.GetRepository<Warehouse>().GetPagingListAsync(page: page, size: size);
            var paginateResponse = new Paginate<GetWarehouseResponse>
            {
                Page = warehouses.Page,
                Size = warehouses.Size,
                Total = warehouses.Total,
                TotalPages = warehouses.TotalPages,
                Items = _mapper.Map<IList<GetWarehouseResponse>>(warehouses.Items)
            };
            return paginateResponse;
        }
    
        public async Task<bool> UpdateWarehouseById(Guid id, UpdateWarehouseRequest updateWarehouseRequest)
        {
            var WarehouseInfor = await _uof.GetRepository<Warehouse>().SingleOrDefaultAsync(predicate: x => x.Id == id);
            if (WarehouseInfor == null)
            {
                throw new Exception("Warehouse not found!");
            }
            WarehouseInfor = _mapper.Map<Warehouse>(updateWarehouseRequest);
            _uof.GetRepository<Warehouse>().UpdateAsync(WarehouseInfor);
            bool isUpdate = await _uof.CommitAsync() > 0;
            return isUpdate;
        }
    }
}
