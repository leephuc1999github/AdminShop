using admin.Models;
using admin.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using view_model.Common;

namespace admin_webapp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderApiClient _orderApiClient;
        public OrderController(IOrderApiClient orderApiClient)
        {
            _orderApiClient = orderApiClient;
        }
        public async Task<IActionResult> Index(GetOrderPagingRequest request)
        {
            List<StatusOrderViewModel> statusList = new List<StatusOrderViewModel>()
            {
                new StatusOrderViewModel(){Id=0,Name="Chờ xác nhận",Value= "ToPay"},
                new StatusOrderViewModel(){Id=1,Name="Chờ lấy hàng",Value="ToShip"},
                new StatusOrderViewModel(){Id=2,Name="Đang giao",Value="ToReceive"},
                new StatusOrderViewModel(){Id=3,Name="Đã giao",Value="Completed"},
                new StatusOrderViewModel(){Id=4,Name="Đã hủy" ,Value="Cancelled"},
                new StatusOrderViewModel(){Id=5,Name="Trả hàng",Value="ReturnRefund"}
            };
            ViewBag.StatusOrder = statusList;
            var data = await _orderApiClient.GetPagings(request);
            data = data.Skip((request.PageIndex - 1) * 5).Take(5).ToList();
            var result = new PagedResult<OrderResponse>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = 5,
                TotalRecords = data.Count
            };
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string id , string note , string status)
        {
            if(id!=null && !id.Equals(""))
            {
                int idOrder = Convert.ToInt32(id);
                var result = await _orderApiClient.UpdateStatus(idOrder , note , status);
                if (result)
                {
                    TempData["Success"] = "Update Success";
                }
                else
                {
                    TempData["Failure"] = "Update Fail";
                }
            }
            return RedirectToAction("","order");
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _orderApiClient.GetOrderById(id);
            if(result != null)
            {
                return View(result);
            }
            return RedirectToAction("","order");
        }
    }
}
