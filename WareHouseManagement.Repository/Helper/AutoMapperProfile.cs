using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Role;
using WareHouseManagement.Repository.Dtos.Request.User;
using WareHouseManagement.Repository.Dtos.Response.Authen;
using WareHouseManagement.Repository.Dtos.Response.Role;
using WareHouseManagement.Repository.Dtos.Response.Shipper;
using WareHouseManagement.Repository.Dtos.Response.User;
using WareHouseManagement.Repository.Models;



namespace WareHouseManagement.Repository.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {


            // Role 
            CreateMap<Role, RoleResponse>().ReverseMap();
            CreateMap<Role, UpdateRoleRequest>().ReverseMap();


            //User
            CreateMap<Account, CreateUserRequest>().ReverseMap();
            CreateMap<Account, UpdateUserRequest>().ReverseMap();
            CreateMap<GetAccountResponse, Account>().ReverseMap();  


            // Authen

            CreateMap<ShipperLoginResponse, Shipper>().ReverseMap();
            CreateMap<WarehouseLoginResponse, Warehouse>().ReverseMap();


            // Shipper
            CreateMap<GetShipperResponse, Shipper>().ReverseMap();


            //Warehouse 

        }
    }
}
