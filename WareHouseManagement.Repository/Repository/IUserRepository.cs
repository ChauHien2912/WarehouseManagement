using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using WareHouseManagement.Repository.Models;


namespace WareHouseManagement.Repository.Repository
{
    public interface IUserRepository
    {
        Account GetAccount(string email, string password);
    }
}
