using System;
using System.Collections.Generic;
using System.Text;

namespace view_model.Utilities.Menus
{
    public class MenuItemVm
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public int? ParentId { get; set; }
        public string Text { get; set; }
        public string Icon { get; set; }
        public string State { get; set; }
        public string Li_Attr { get; set; }
        public string A_Attr { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
