using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.API.Constant;
using WareHouseManagement.Repository.Dtos.Request.Shippper;
using WareHouseManagement.Repository.Services.IServices;

namespace WareHouseManagement.API.Controllers
{
    [Route(APIEndPointConstant.Shipper.ShipperEndpoint+ "/[action]")]
    [ApiController]
    public class ShipperController : BaseController
    {
        private readonly IShipperService _shipperservice;

        public ShipperController(IMapper _mapper, IShipperService shipperservice) : base(_mapper)
        {
            _shipperservice = shipperservice;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetShipperById([FromRoute]Guid id)
        {
            try
            {
                var result = await _shipperservice.GetShipperById(id);
                if(result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{warehouseid:Guid}")]
        public async Task<IActionResult> GetShippers([FromRoute]Guid warehouseid,int page, int size)
        {
            try
            {
                var result = await _shipperservice.GetShippers(warehouseid, page, size);
                if(result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateShipperById([FromRoute]Guid id, UpdateShipperRequest request)
        {
            try
            {
                var result = await _shipperservice.UpdateShipper(id, request);
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

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteShipperbyId([FromRoute] Guid id)
        {
            try
            {
                var result = await _shipperservice.DeleteShipperById(id);
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
        public async Task<IActionResult> GetOrderOfShipperByBatchMode([FromRoute] Guid id, [FromQuery]string batchmode, [FromQuery]int page = 1, [FromQuery]int size = 10)
        {
            try
            {
                var result = await _shipperservice.GetOrderOfShipperByImported(id, batchmode, page, size);
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
