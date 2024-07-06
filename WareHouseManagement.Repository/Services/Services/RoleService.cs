using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Role;
using WareHouseManagement.Repository.Dtos.Response.Role;
using WareHouseManagement.Repository.Entities;

//using WareHouseManagement.Repository.Models;
using WareHouseManagement.Repository.Repository;
using WareHouseManagement.Repository.Services.IServices;
using WareHouseManagement.Repository.Specifications;

namespace WareHouseManagement.Repository.Services.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork uow, IMapper mapper)
        {

            _mapper = mapper;
            _uow = uow;
        }

        public async Task<bool> CreateRole(CreateRoleRequest request)
        {

            var existingRole = await _uow.GetRepository<Role>().SingleOrDefaultAsync(predicate: e => e.RoleName == request.RoleName);
            bool isStatus = false;
            if(existingRole != null)
            {
                return false;

            } else
            {
                Role role = new Role
                {
                    RoleName = request.RoleName
                };

                await _uow.GetRepository<Role>().InsertAsync(role);
                await _uow.CommitAsync();
                isStatus = true;
            }
            
            

            return isStatus;
        }

        public async Task DeleteRole(Guid id)
        {
            var existingRole = await _uow.GetRepository<Role>().SingleOrDefaultAsync(predicate: e => e.Id == id);
            if (existingRole == null)
            {
                throw new Exception("Not Found");
            }

             _uow.GetRepository<Role>().DeleteAsync(existingRole);
            _uow.CommitAsync();

        }

        public async Task<IPaginate<RoleResponse>> GetRoles(int page, int size)
        {
            var role = await _uow.GetRepository<Role>().GetPagingListAsync(page: page, size: size);

            var roleresponse = new Paginate<RoleResponse>()
            {
                Page = role.Page,
                Size = role.Size,
                Total = role.Total,
                TotalPages = role.TotalPages,
                Items = _mapper.Map<IList<RoleResponse>>(role.Items)
             };
            return roleresponse;
        }

        public async Task<bool> UpdateRoleById(Guid id,UpdateRoleRequest request)
        {
            var existingRole = await _uow.GetRepository<Role>().SingleOrDefaultAsync(predicate: e => e.Id == id);
            if (existingRole == null)
            {
                throw new Exception("Not Found");

            }

            existingRole = _mapper.Map<Role>(request);
            _uow.GetRepository<Role>().UpdateAsync(existingRole);
            bool isUpdate = await _uow.CommitAsync() > 0;
            return isUpdate;
        }
    }
}
