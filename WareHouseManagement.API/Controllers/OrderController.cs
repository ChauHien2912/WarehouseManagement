﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.API.Constant;
using WareHouseManagement.Repository.Dtos.Request.Order;
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
        [Route("{warehouseid:Guid}")]
        public async Task<IActionResult> UpdateBatchModeByWarehouse([FromRoute] Guid warehouseid)
        {
            try
            {
                var result = await _service.UpdataBatchModeByWarehouse(warehouseid);
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


    }
}
