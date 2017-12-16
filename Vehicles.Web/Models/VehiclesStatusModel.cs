using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vehicles.Web.Models
{
    public class VehiclesStatusModel
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string VIN { get; set; }
        public string RegNo { get; set; }
        public DateTime LastPingTime { get; set; }
        public bool IsActive { get; set; }
    }
}
