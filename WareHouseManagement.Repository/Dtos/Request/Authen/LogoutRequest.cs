using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Request.Authen
{
    public class LogoutRequest
    { 
        public string RefreshToken { get; set; }
    }
}
