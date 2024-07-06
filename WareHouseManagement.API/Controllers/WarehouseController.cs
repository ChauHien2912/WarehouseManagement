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


        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWarehouseById([FromRoute]Guid id)
        {
            try
            {
                var result = await _warehouseService.GetWarehouseById(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWarehouseById([FromRoute]Guid id)
        {
            try
            {
                var result = await _warehouseService.DeleteWarehouseById(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetWarehouses(int page, int size)
        {
            try
            {
                var result = await _warehouseService.GetWarehouses(page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateWarehouse([FromRoute]Guid id, UpdateWarehouseRequest request)
        {
            try
            {
                var result = await _warehouseService.UpdateWarehouseById(id, request);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRevenueSuccessByWarehouseId([FromRoute] Guid id, [FromQuery] int day, [FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var result = await _warehouseService.GetRevenueBySuccess(id,day, month, year);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRevenueImportedByWarehouseId([FromRoute] Guid id, [FromQuery]int day ,[FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var result = await _warehouseService.GetRevenueByImprorted(id,day, month, year);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetorderFailByWarehouseId([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = await _warehouseService.GetOrderFailByWarehouse(id, page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetorderSuccessByWarehouseId([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = await _warehouseService.GetOrderSuccessByWarehouse(id, page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetShippers([FromRoute] Guid id)
        {
            try
            {
                var result = await _warehouseService.GetShippers(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetShippersInWarehouseId([FromRoute] Guid id, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = await _warehouseService.GetShipperByWarehouse(id, page,size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("{id:Guid}")]

        public async Task<IActionResult> ExportFileOrderExported([FromRoute]Guid id)
        {
            try
            {
                var result = await _warehouseService.ExportFileExcelOrderFail(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCountImportedInDate([FromRoute] Guid id, [FromQuery] int day, [FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var result = await _warehouseService.GetOrderImportedInDate(id, day, month, year);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCountExportedInDate([FromRoute] Guid id, [FromQuery] int day, [FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var result = await _warehouseService.GetOrderExportedInDate(id, day, month, year);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRateSuccessOfOrder([FromRoute] Guid id)
        {
            try
            {
                var result = await _warehouseService.GetRateSuccessOfOrder(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetRateFailOfOrder([FromRoute] Guid id)
        {
            try
            {
                var result = await _warehouseService.GetRateFailOfOrder(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetNumberOfOrder([FromRoute] Guid id)
        {
            try
            {
                var result = await _warehouseService.GetTotalOrdersByWarehouse(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
