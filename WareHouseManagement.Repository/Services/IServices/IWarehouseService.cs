using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.WareHouse;
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
        //Task<IPaginate<SearchWarehousenByNameAddressServiceResponse>> SearchSalonByNameAddressService(int page, int size, string? salonAddress = "");
    }
}
