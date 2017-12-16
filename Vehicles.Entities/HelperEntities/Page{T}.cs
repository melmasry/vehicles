using System.Collections.Generic;

namespace Vehicles.Entities.HelperEntities
{
    public sealed class Page<T>
    {
        public IEnumerable<T> Items { get; set; }

        public long TotalSize { get; set; }
        
    }
}
