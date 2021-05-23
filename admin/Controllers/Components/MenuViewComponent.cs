using admin_webapp.Models;
using admin_webapp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using view_model.Utilities.Menus;

namespace admin_webapp.Controllers.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuApiClient _menuApiClient;
        public MenuViewComponent(IMenuApiClient menuApiClient)
        {
            _menuApiClient = menuApiClient;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menuItems = await _menuApiClient.GetAll();
            return View("Default", menuItems.Where(mi => mi.Type.Equals("admin")).Select(item => new MenuItemViewModel()
            {
                id = item.Id.ToString(),
                text = item.Text,
                parent = item.ParentId.ToString(),
                type = item.Type,
                url = item.Url
            }).ToList());
        }
    }
}
