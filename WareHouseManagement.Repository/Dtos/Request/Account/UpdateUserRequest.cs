﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Request.User
{
    public class UpdateUserRequest
    {
        public string FullName { get; set; } = null!;
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Cccd { get; set; }
    }
}
