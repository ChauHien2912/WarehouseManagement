using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Role;
using WareHouseManagement.Repository.Dtos.Response.Role;

using WareHouseManagement.Repository.Repository;
using WareHouseManagement.Repository.Specifications;

namespace WareHouseManagement.Repository.Services.IServices
{
    public interface IRoleService
    {
        Task<bool> CreateRole(CreateRoleRequest request);
        Task<bool> UpdateRoleById(int id, UpdateRoleRequest request);

        Task DeleteRole(int id);

        Task<IPaginate<RoleResponse>> GetRoles(int page, int size);
    }
}
