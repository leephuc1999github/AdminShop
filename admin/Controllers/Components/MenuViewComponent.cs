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
            return View("Default");
        }
    }
}
