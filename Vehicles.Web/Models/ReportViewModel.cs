using System;
using System.Collections.Generic;
using System.Linq;

namespace Vehicles.Web.Models
{
    public class ReportViewModel
    {
        public List<JqueryReportHeader> Headers;

        public List<Dictionary<string, object>> Data { get; set; }
    }

    public class JqueryReportHeader
    {
        // Must be named "data" for the JQuery datatable to understand the column name
        public string data { get; set; }

        // Must be named "bSearchable" for the JQuery datatable to understand which columns will be searhable
        public bool bSearchable { get; set; }

        public JqueryReportHeader(string header)
        {
            data = header;
            bSearchable = true;
        }
    }
}