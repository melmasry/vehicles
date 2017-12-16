using System.Linq;
using Vehicles.Entities.DomainEntities;
using Vehicles.Entities.HelperEntities;

namespace Vehicles.Api.Tests.Helpers
{
    public static class EqualityHelper
    {
        public static bool SamePageOfCustomers(Page<Customer> page1, Page<Customer> page2)
        {
            var equal = page1.TotalSize.Equals(page2.TotalSize);
            equal &= page1.Items.Count() == page2.Items.Count();
            var items1 = page1.Items.ToArray();
            var items2 = page2.Items.ToArray();
            for (int i = 0; i < items1.Count(); i++)
                equal &= SameCustomers(items1[i], items2[i]);
            return equal;
        }

        public static bool SamePageOfVehicles(Page<Vehicle> page1, Page<Vehicle> page2)
        {
            var equal = page1.TotalSize.Equals(page2.TotalSize);
            equal &= page1.Items.Count() == page2.Items.Count();
            var items1 = page1.Items.ToArray();
            var items2 = page2.Items.ToArray();
            for (int i = 0; i < items1.Count(); i++)
                equal &= SameVehicles(items1[i], items2[i]);
            return equal;
        }

        public static bool SameCustomers(Customer customer1, Customer customer2)
        {
            var equal = customer1.Id.Equals(customer2.Id) &&
                        customer1.Name.Equals(customer2.Name);
            if (!equal || (customer1.Vehicles == null && customer2.Vehicles == null))
                return equal;

            equal &= customer1.Vehicles.Count() == customer2.Vehicles.Count();

            var items1 = customer1.Vehicles.ToArray();
            var items2 = customer2.Vehicles.ToArray();
            for (int i = 0; i < items1.Count(); i++)
                equal &= SameVehicles(items1[i], items2[i]);
            
            return equal;
        }

        public static bool SameVehicles(Vehicle vehicle1, Vehicle vehicle2)
        {
            var equal = vehicle1.VIN.Equals(vehicle2.VIN) &&
                        vehicle1.RegNo.Equals(vehicle2.RegNo);
            return equal;
        }
    }
}
