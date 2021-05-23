using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace admin_webapp.Models
{
    public class NavigationViewModel
    {
        public List<SelectListItem> Languages { get; set; }
        public List<string> Icons { get; set; }

        public string CurrentLanguageId { get; set; }

        public string ReturnUrl { set; get; }
    }
}
