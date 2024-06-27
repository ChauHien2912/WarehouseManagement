using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.API.Constant;
using WareHouseManagement.Repository.Dtos.Request.WareHouse;
using WareHouseManagement.Repository.Services.IServices;

namespace WareHouseManagement.API.Controllers
{
    [Route(APIEndPointConstant.Warehouse.WarehouseEndpoint+ "/[action]")]
    [ApiController]
    public class WarehouseController : BaseController
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService, IMapper mapprer) : base(mapprer)
        {
            _warehouseService = warehouseService;
        }


        //[HttpGet]
        //[Route("{id}")]
        //public async Task<IActionResult> GetWarehouseById([FromRoute]int id)
        //{
        //    try
        //    {
        //        var result = await _warehouseService.GetWarehouseById(id);
        //        if(result == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(result);
        //    }catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //[HttpDelete]
        //[Route("{id}")]
        //public async Task<IActionResult> DeleteWarehouseById([FromRoute] int id)
        //{
        //    try
        //    {
        //        var result = await _warehouseService.DeleteWarehouseById(id);
        //        if (result == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //[HttpGet]
        
        //public async Task<IActionResult> GetWarehouses(int page, int size)
        //{
        //    try
        //    {
        //        var result = await _warehouseService.GetWarehouses(page, size);
        //        if (result == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //[HttpPut]
        //[Route("{int}")]
        //public async Task<IActionResult> UpdateWarehouse([FromRoute]int id, UpdateWarehouseRequest request)
        //{
        //    try
        //    {
        //        var result = await _warehouseService.UpdateWarehouseById(id, request);
        //        if (result == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
