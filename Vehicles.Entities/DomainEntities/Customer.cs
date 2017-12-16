using System.Collections.Generic;

namespace Vehicles.Entities.DomainEntities
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
