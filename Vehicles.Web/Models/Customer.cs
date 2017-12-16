using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vehicles.Web.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLn1 { get; set; }
        public string AddressLn2 { get; set; }
        public string AddressLn3 { get; set; }
        public string Phone { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}
