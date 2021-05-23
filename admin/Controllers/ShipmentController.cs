using admin_webapp.Models;
using admin_webapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using view_model.Catalog.Shipments;

namespace admin_webapp.Controllers
{
    public class ShipmentController : BaseController
    {
        private readonly IShipmentApiClient _shipmentApiClient;
        public ShipmentController(IShipmentApiClient shipmentApiClient) 
        {
            _shipmentApiClient = shipmentApiClient;
        }
        public async Task<IActionResult> Index(string code, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetShipmentPagingRequest()
            {
                Code = code,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            List<StatusShipmentViewModel> statusList = new List<StatusShipmentViewModel>()
            {
                new StatusShipmentViewModel(){Id=0,Name="Pending",Value=0},
                new StatusShipmentViewModel(){Id=1,Name="Approved",Value=1},
                new StatusShipmentViewModel(){Id=2,Name="Delivering",Value=2},
                new StatusShipmentViewModel(){Id=3,Name="Complete",Value=3},
                new StatusShipmentViewModel(){Id=4,Name="Canceled",Value=4},
            };
            ViewBag.StatusShipment = statusList;
            var data = await _shipmentApiClient.GetPagings(request);
            return View(data.ResultObj);
        }
    }
}
