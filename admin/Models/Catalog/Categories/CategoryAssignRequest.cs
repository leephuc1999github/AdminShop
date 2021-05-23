using System;
using System.Collections.Generic;
using System.Text;
using view_model.Common;

namespace view_model.Catalog.Categories
{
    public class CategoryAssignRequest
    {
        public int Id { get; set; }
        public List<SelectItem> Categories { get; set; } = new List<SelectItem>();

    }
}
