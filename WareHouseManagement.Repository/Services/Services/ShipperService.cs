using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Shippper;
using WareHouseManagement.Repository.Dtos.Response.Shipper;
using WareHouseManagement.Repository.Entities;


//using WareHouseManagement.Repository.Models;
using WareHouseManagement.Repository.Repository;
using WareHouseManagement.Repository.Services.IServices;
using WareHouseManagement.Repository.Specifications;

namespace WareHouseManagement.Repository.Services.Services
{
    public class ShipperService : IShipperService
    {
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public ShipperService(IUnitOfWork uof, IConfiguration config, IMapper mapper)
        {
            _uof = uof;
            _config = config;
            _mapper = mapper;
        }

        public async Task<bool> DeleteShipperById(Guid id)
        {
            var shipper = await _uof.GetRepository<Shipper>().SingleOrDefaultAsync(predicate: p => p.Id == id);
            var accountid = shipper.AccountId;
            if(shipper == null)
            {
                throw new Exception("Cannot Find Shipper");
            }
            if(accountid == null)
            {
                throw new Exception("Cannot find account");
            }
            var account = await _uof.GetRepository<Account>().SingleOrDefaultAsync(predicate: p =>  p.Id == accountid);

             _uof.GetRepository<Shipper>().DeleteAsync(shipper);
             _uof.GetRepository<Account>().DeleteAsync(account);
            bool isDeleted = await _uof.CommitAsync() > 0;
            return isDeleted;


        }

        public async Task<GetShipperResponse> GetShipperById(Guid id)
        {
            try
            {
                var existing = await _uof.GetRepository<Shipper>().SingleOrDefaultAsync(predicate: e => e.Id == id);
                if (existing == null)
                {
                    return null;
                }
                return _mapper.Map<GetShipperResponse>(existing);
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IPaginate<GetShipperResponse>> GetShippers(Guid warehouseid,int page, int size)
        {
            var shippers = await _uof.GetRepository<Shipper>().GetPagingListAsync(
                predicate: p => p.Id == warehouseid,
                page: page, size: size,
                include: i => i.Include(p => p.Warehouse)
                );
            var paginateResponse = new Paginate<GetShipperResponse>
            {
                Page = shippers.Page,
                Size = shippers.Size,
                Total= shippers.Total,
                TotalPages = shippers.TotalPages,
                Items = _mapper.Map<IList<GetShipperResponse>>(shippers.Items)
            };
            return paginateResponse;
        }

        public async Task<bool> UpdateShipper(Guid id, UpdateShipperRequest request)
        {
            var shipper = await _uof.GetRepository<Shipper>().SingleOrDefaultAsync(predicate: p => p.Id == id);
            if(shipper == null)
            {
                return false;
            }
            shipper = _mapper.Map<Shipper>(request);
            shipper.Id = id;
            _uof.GetRepository<Shipper>().UpdateAsync(shipper);
            bool isUpdated = await _uof.CommitAsync() > 0;
            return isUpdated;
        }
    }
}
