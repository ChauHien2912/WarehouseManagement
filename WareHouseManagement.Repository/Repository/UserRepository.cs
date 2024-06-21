using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Models;
//using WareHouseManagement.Repository.Models;


namespace WareHouseManagement.Repository.Repository
{
    public class UserRepository : GenericRepository<Account>, IUserRepository
    {
        private readonly OrderManagementContext _context;

        public UserRepository(OrderManagementContext context) : base(context)
        {
            _context = context;
        }

        public Account GetAccount(string username, string password)
        {
            return _context.Accounts.FirstOrDefault(a => a.Email == username && a.Password == password);
        }

    }
}
