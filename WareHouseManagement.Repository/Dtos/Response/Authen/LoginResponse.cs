﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WareHouseManagement.Repository.Dtos.Response.Authen
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public Guid AccountId { get; set; }
        
        public string RoleName { get; set; }

        public ShipperLoginResponse ShipperResponse { get; set; }
        public WarehouseLoginResponse WarehouseResponse { get; set; }

        public AdminLoginResponse AdminResponse { get; set; }

    }


    public class AdminLoginResponse
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        
    }
    public class ShipperLoginResponse
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public string? Phone { get; set; }
        public Guid? WarehouseId { get; set; }
    }

    public class WarehouseLoginResponse
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
    }
}
