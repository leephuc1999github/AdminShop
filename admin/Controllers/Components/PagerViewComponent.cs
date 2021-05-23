using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using view_model.Common;

namespace admin_webapp.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
