using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace admin_webapp.Models
{
    public class MenuItemViewModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public string parent { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }
}
