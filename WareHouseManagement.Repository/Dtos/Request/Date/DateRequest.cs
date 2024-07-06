using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Dtos.Request.NewFolder
{
    public class DateRequest
    {
        public int? Day {  get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }
    }
}
