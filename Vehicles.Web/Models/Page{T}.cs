using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vehicles.Web.Models
{
    public sealed class Page<T>
    {
        public IEnumerable<T> Items { get; set; }

        public long TotalSize { get; set; }
    }
}
