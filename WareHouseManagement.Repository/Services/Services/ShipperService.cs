using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
