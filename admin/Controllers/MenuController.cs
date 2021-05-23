using admin_webapp.Models;
using admin_webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin_webapp.Controllers
{
    [Authorize(Roles="admin")]
    public class MenuController : BaseController
    {
        private readonly IMenuApiClient _menuApiClient;
        public MenuController(IMenuApiClient menuApiClient) 
        {
            _menuApiClient = menuApiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _menuApiClient.GetAll();
            List<MenuItemViewModel> menuItemsAdmin = new List<MenuItemViewModel>();
            List<MenuItemViewModel> menuItemsSell = new List<MenuItemViewModel>();
            foreach (var item in result)
            {
                if (item.Type.Equals("admin"))
                {
                    menuItemsAdmin.Add(new MenuItemViewModel()
                    {
                        id = item.Id + "",
                        text = item.Text,
                        parent = (item.ParentId == null ? "#" : (item.ParentId + "")),
                        type = item.Type
                    });
                }
                else
                {
                    menuItemsSell.Add(new MenuItemViewModel()
                    {
                        id = item.Id + "",
                        text = item.Text,
                        parent = (item.ParentId == null ? "#" : (item.ParentId + "")),
                        type = item.Type
                    });
                }
                   
            }
            ViewBag.MenuItemsAdmin = JsonConvert.SerializeObject(menuItemsAdmin);
            ViewBag.MenuItemsSell = JsonConvert.SerializeObject(menuItemsSell);
            return View();
        }
    }
}
