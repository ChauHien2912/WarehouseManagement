using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.WareHouse;
using WareHouseManagement.Repository.Dtos.Response.Shipper;
using WareHouseManagement.Repository.Dtos.Response.WareHouse;
using WareHouseManagement.Repository.Models;

//using WareHouseManagement.Repository.Models;
using WareHouseManagement.Repository.Repository;
using WareHouseManagement.Repository.Services.IServices;
using WareHouseManagement.Repository.Specifications;

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
