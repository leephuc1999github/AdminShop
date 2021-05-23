using System;
using System.Collections.Generic;

namespace view_model.Catalog.Products
{
    public class ProductVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string LanguageId { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Price { get; set; }
        public string SeoDescription { set; get; }
        public string SeoTitle { set; get; }

        public string SeoAlias { get; set; }
        public int Stock { get; set; }
        public int ViewCount { get; set; }
        public string ThumbnailImage { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
    }
}
