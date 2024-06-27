using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WareHouseManagement.API.Constant;

namespace WareHouseManagement.API.Controllers
{
    [Route(APIEndPointConstant.ApiEndpoint)]
    [ApiController]
    //public class BaseController<T> : ControllerBase where T : BaseController<T>
    //{
    //    protected ILogger<T> _logger;
    //    public BaseController(ILogger<T> logger)
    //    {
    //        _logger = logger;
    //    }
    //}
    public class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
