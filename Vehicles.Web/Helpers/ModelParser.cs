using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Vehicles.Web.Models;

namespace Vehicles.Web.Helpers
{
    public static class ModelParser
    {
        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        public static List<Dictionary<string, object>> GetCustomersRows(IEnumerable<Customer> customers)
        {
            var lstRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> dictRow = new Dictionary<string, object>();

            foreach(Customer customer in customers)
                foreach(Vehicle vehicle in customer.Vehicles)
                {
                    dictRow = new Dictionary<string, object>();
                    dictRow.Add("CustomerName", customer.Name);
                    dictRow.Add("Address", string.Format("{0}, {1}, {2}", customer.AddressLn1,customer.AddressLn2 , customer.AddressLn3));
                    dictRow.Add("Phone", customer.Phone);
                    dictRow.Add("VIN", vehicle.VIN);
                    dictRow.Add("RegNo", vehicle.RegNo);
                    dictRow.Add("LastPingTime", vehicle.LastPingTime);
                    dictRow.Add("IsActive", vehicle.IsActive);
                    lstRows.Add(dictRow);
                }
            
            return lstRows;
        }

        public static List<JqueryReportHeader> GetCustomersHeaders(IEnumerable<Customer> customers)
        {
            List<JqueryReportHeader> lstHeaders = new List<JqueryReportHeader>();
            lstHeaders.Add(new JqueryReportHeader("CustomerName"));
            lstHeaders.Add(new JqueryReportHeader("Address"));
            lstHeaders.Add(new JqueryReportHeader("Phone"));
            lstHeaders.Add(new JqueryReportHeader("VIN"));
            lstHeaders.Add(new JqueryReportHeader("RegNo"));
            lstHeaders.Add(new JqueryReportHeader("LastPingTime"));
            lstHeaders.Add(new JqueryReportHeader("IsActive"));
            return lstHeaders;
        }
    }
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }
}
