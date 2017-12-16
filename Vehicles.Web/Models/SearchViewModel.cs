using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Vehicles.Web.Models
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
        }

        [Description("CustomerName")]
        public string CustomerName { get; set; }

        [Description("Status")]
        public string Status { get; set; }
    }
}
