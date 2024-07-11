using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.API.Constant;
using WareHouseManagement.Repository.Dtos.Request.Order;
using WareHouseManagement.Repository.Dtos.Request.WareHouse;
using WareHouseManagement.Repository.Services.IServices;

namespace WareHouseManagement.API.Controllers
{
    [Route(APIEndPointConstant.Order.OrderEndpoint + "/[action]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service, IMapper mapper):base(mapper) {
            _service = service;
              
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromForm]CreateOrderRequest request)
        {
            try
            {
                var result = await _service.CreateOrder(request);
                if(result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = await _service.GetOrders(page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetOrderById([FromRoute]Guid id)
        {
            try
            {
                var result = await _service.GetOrderById(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        
        public async Task<IActionResult> UpdateBatchModeByWarehouse([FromBody] UpdateBatchMode request)
        {
            try
            {
                var result = await _service.UpdataBatchModeByWarehouse(request);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok($"Bạn đã nhập thành công {result} hàng vào kho");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{shipperid:Guid}")]
        public async Task<IActionResult> GetOrderByShipperid([FromRoute] Guid shipperid, int page = 1, int size = 10)
        {
            try
            {
                var result = await _service.GetOrderByShipper(shipperid, page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetListBatchByWarehouse([FromRoute] Guid id, int page = 1, int size = 10)
        {
            try
            {
                var result = await _service.GetBatchByWarehouseId(id, page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetListOrdersByBatch([FromRoute] Guid id, int page = 1, int size = 10)
        {
            try
            {
                var result = await _service.GetListOrderByBatchId(id, page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBatchModebyShipper([FromForm]UpdateBactchModeRequest request)
        {
            try
            {
                var result = await _service.UpdateBatchModeByShipper(request);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetOrderImported([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = await _service.GetOrderImported(page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderTruckOut([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = await _service.GetOrderTruckOut(page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderFail([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = await _service.GetOrderFail(page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderSuccess([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = await _service.GetOrderSuccess(page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetOrderDelivering([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = await _service.GetOrderDeliveing(page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetOrderTruckin([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = await _service.GetOrderTruckIn(page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetOrderOfWarehouseByBatchMode([FromRoute] Guid id, [FromQuery]string BatchMode, [FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            try
            {
                var result = await _service.GetOrderOfWarehouseByBatchMode(id, BatchMode, page, size);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
