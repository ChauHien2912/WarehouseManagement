﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Response.Order
{
    public class GetOrderResponse
    {
        public Guid Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ExpectedDateOfDelivery { get; set; }
        public decimal? Price { get; set; }
        public Guid? WarehouseId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Img { get; set; }

        // Additional properties for Batch information
        public string? BatchMode { get; set; }
        public Guid? ShipperId { get; set; }
        public Guid? BatchId { get; set; }


    }
}
