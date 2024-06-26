using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.API.Constant;
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
        public async Task<IActionResult> GetShippers(int page, int size)
        {
            try
            {
                var result = await _shipperservice.GetShippers(page, size);
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
    }
}
