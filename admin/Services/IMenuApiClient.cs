using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using view_model.Utilities.Menus;

namespace admin_webapp.Services
{
    public interface IMenuApiClient
    {
        Task<List<MenuItemVm>> GetAll();
    }
}
