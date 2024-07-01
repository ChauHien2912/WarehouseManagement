using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseManagement.Repository.Dtos.Request.Account;
using WareHouseManagement.Repository.Dtos.Request.Role;
using WareHouseManagement.Repository.Dtos.Request.User;
using WareHouseManagement.Repository.Dtos.Request.WareHouse;
using WareHouseManagement.Repository.Dtos.Response.Account;
using WareHouseManagement.Repository.Dtos.Response.Authen;
using WareHouseManagement.Repository.Dtos.Response.Order;
using WareHouseManagement.Repository.Dtos.Response.Role;
using WareHouseManagement.Repository.Dtos.Response.Shipper;
using WareHouseManagement.Repository.Dtos.Response.User;
using WareHouseManagement.Repository.Dtos.Response.WareHouse;
using WareHouseManagement.Repository.Models;
//using WareHouseManagement.Repository.Models;



namespace WareHouseManagement.Repository.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {


            //Account

            CreateMap<Shipper, GetAccountResponse>().ReverseMap();
            CreateMap<Warehouse, GetAccountResponse>().ReverseMap();

            // Role 
            CreateMap<Role, RoleResponse>().ReverseMap();
            CreateMap<Role, UpdateRoleRequest>().ReverseMap();


            ////User
            CreateMap<Account, CreateUserRequest>().ReverseMap();
            CreateMap<Account, UpdateUserRequest>().ReverseMap();
            CreateMap<GetAccountResponse, Account>().ReverseMap();
            CreateMap<GetAccountResponse, Account>().ReverseMap();
            CreateMap<GetAccountResponse, Account>().ReverseMap();
            CreateMap<CreateAccountRequest, Shipper>().ReverseMap();
            CreateMap<CreateAccountRequest, Warehouse>().ReverseMap();
            CreateMap<CreateAccountRequest, Account>().ReverseMap();
            CreateMap<CreateAccountRequest, CreateAccountResponse>().ReverseMap();


            //// Authen

            CreateMap<ShipperLoginResponse, Shipper>().ReverseMap();
            CreateMap<WarehouseLoginResponse, Warehouse>().ReverseMap();


            //// Shipper
            CreateMap<GetShipperResponse, Shipper>().ReverseMap();


            //Warehouse 
            CreateMap<GetWarehouseResponse, Warehouse>().ReverseMap();
            CreateMap<Warehouse, UpdateWarehouseRequest>().ReverseMap();
            CreateMap<CreateWarehouseResponse, Warehouse>().ReverseMap();



            //Order
            CreateMap<Order, GetOrderResponse>().ReverseMap();
            //CreateMap<BatchOrder, GetOrderResponse>().ReverseMap();
            CreateMap<BatchOrder, GetOrderResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Order.Id))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.Order.OrderDate))
            .ForMember(dest => dest.ExpectedDateOfDelivery, opt => opt.MapFrom(src => src.Order.ExpectedDateOfDelivery))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Order.Price))
            .ForMember(dest => dest.WarehouseId, opt => opt.MapFrom(src => src.Order.WarehouseId))
            .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => src.Order.DeliveryDate))
            .ForMember(dest => dest.Img, opt => opt.MapFrom(src => src.Order.Img))
            .ForMember(dest => dest.BatchMode, opt => opt.MapFrom(src => src.Batch.BatchMode))
            .ForMember(dest => dest.ShipperId, opt => opt.MapFrom(src => src.Batch.ShipperId))
            .ForMember(dest => dest.BatchId, opt => opt.MapFrom(src => src.BatchId));
        }
    }
}
