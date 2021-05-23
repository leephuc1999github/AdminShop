using System;
using System.Collections.Generic;
using System.Text;

namespace view_model.Utilities.Menus
{
    public class CreateMenuItemRequest
    {
        public string Text { get; set; }
        public int? ParentId { get; set; }
        public string Type { get; set; } 
        public int SortOrder { get; set; }
    }
}
