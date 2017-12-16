using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vehicles.Web.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string VIN { get; set; }
        public string RegNo { get; set; }
        public DateTime LastPingTime { get; set; }
        public bool IsActive { get; set; }
    }
}
