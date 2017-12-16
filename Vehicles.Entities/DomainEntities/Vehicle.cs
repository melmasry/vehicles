using System;

namespace Vehicles.Entities.DomainEntities
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
