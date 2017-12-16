using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vehicles.Web.Models;
using Vehicles.Web.Helpers;
using System.Net.Http;
using Microsoft.Extensions.Options;
using System.Net;
using System.IO;

namespace Vehicles.Web.Controllers
{
    public class VehiclesStatusController : Controller
    {
        private UrlModel _urlModel;
        public VehiclesStatusController(IOptions<UrlModel> urlModel)
        {
            _urlModel = urlModel.Value;
        }
        [ActionName("ViewReport")]
        public IActionResult VehiclesStatus()
        {
            return Initialize("~/Views/Home/VehicleStatus.cshtml");
        }
        public IActionResult Initialize(string viewPath)
        {
            var viewModel = new SearchViewModel();

            return View(viewPath, viewModel);
        }
        public IActionResult Search(SearchViewModel model)
        {
            var result = DoSearch(model).Result;
            return PartialView("ReportGrid", result.Headers);
        }
        public ActionResult GetSearchResult()
        {
            var gridViewModel = (ReportViewModel)TempData.Get<ReportViewModel>("gridViewModel");
            return Json(new { resources = (gridViewModel!=null)?gridViewModel.Data:null, Columns = (gridViewModel != null) ? gridViewModel.Headers:null });
        }
        public async Task<ReportViewModel> DoSearch(SearchViewModel model)
        {
            string url = string.Empty;
            url = _urlModel.Url + "customers/name?";
            //if(model.CustomerName!= null && model.CustomerName!= string.Empty)
            //    url += "name =" + model.CustomerName ;
            //if (model.Status != null && model.Status != string.Empty)
            //    url += "&vehicleStatus=" + model.Status;
            url += "name=" + ((model.CustomerName==null)?string.Empty:model.CustomerName);
            url += "&vehicleStatus=" + ((model.Status == null) ? string.Empty : model.Status);

            var req = WebRequest.Create(url);
            var r = await req.GetResponseAsync().ConfigureAwait(false);

            var responseReader = new StreamReader(r.GetResponseStream());
            var responseData = await responseReader.ReadToEndAsync();

            var lstCustomers = Newtonsoft.Json.JsonConvert.DeserializeObject<Page<Customer>>(responseData).Items;

            var gridViewModel = new ReportViewModel
            {
                Data = ModelParser.GetCustomersRows(lstCustomers),
                Headers = ModelParser.GetCustomersHeaders(lstCustomers)
            };

            TempData.Put("gridViewModel", gridViewModel);
            return gridViewModel;
        }
    }
}
