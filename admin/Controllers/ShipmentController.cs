using admin_webapp.Models;
using admin_webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using view_model.Catalog.Shipments;

namespace admin_webapp.Controllers
{
    [Authorize(Roles="admin,employee")]
    public class ShipmentController : BaseController
    {
        private readonly IShipmentApiClient _shipmentApiClient;
        public ShipmentController(IShipmentApiClient shipmentApiClient) 
        {
            _shipmentApiClient = shipmentApiClient;
        }
        public async Task<IActionResult> Index(string code, int pageIndex = 1, int pageSize = 10)
        {
            try
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
                new StatusShipmentViewModel(){Id=3,Name="Complete",Value=3}
            };
                ViewBag.StatusShipment = statusList;
                var data = await _shipmentApiClient.GetPagings(request);
                return View(data.ResultObj);
            }
            catch(Exception e)
            {
                return RedirectToAction("", "error");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id ,int status , string NoteShipping , float moneyShipping)
        {
            var result = await _shipmentApiClient.Update(id, status,NoteShipping, moneyShipping);
            return RedirectToAction("","shipment");
        }

    }
}
