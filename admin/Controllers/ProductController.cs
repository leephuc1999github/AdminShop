using admin_webapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using view_model.Catalog.Categories;
using view_model.Catalog.Products;
using view_model.Common;

namespace admin_webapp.Controllers
{
    [Authorize(Roles = "admin,employee")]
    public class ProductController : BaseController
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        public ProductController(IProductApiClient productApiClient, ICategoryApiClient categoryApiClient)
        {
            _categoryApiClient = categoryApiClient;
            _productApiClient = productApiClient;
        }
        #region all product
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10, int? categoryId = null)
        {
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId
            };
            var data = await _productApiClient.GetPagings(request);
            if (keyword != null )
            {
                data = data.Where(p => p.Name.ToLower().Contains(keyword.ToLower())).ToList();
            }
            if(categoryId != null)
            {
                data = data.Where(p => p.ProductGroup.Catalog.Id == categoryId).ToList();
            }
            var categories = await _categoryApiClient.GetAll();
            List<SelectListItem> optionsCategory = new List<SelectListItem>();
            foreach(var category in categories)
            {
                SelectListItem option = new SelectListItem()
                {
                    Text = category.Name,
                    Value = category.Id + ""
                };
                optionsCategory.Add(option);
            }
            ViewData["OptionsCategory"] = optionsCategory;
            ViewBag.categoryId = categoryId;
            ViewBag.keyword = keyword;
            var result = new PagedResult<ProductResponse>()
            {
                Items = data,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = data.Count
            };
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id  , int pageIndex)
        {
            var result = await _productApiClient.GetById(id);
            var items = new PagedResult<Item>()
            {
                Items = result.Items.Skip((pageIndex - 1) * 3).Take(3).ToList(),
                PageIndex = pageIndex,
                PageSize = 2,
                TotalRecords = result.Items.Count
            };
            ViewBag.Items = items;
            return View(result);
        }
        #endregion

        #region create product
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            
            var productGroups = await _productApiClient.GetAllProductGroup();
            List<SelectListItem> optionsProductGroups = new List<SelectListItem>();
            foreach (var pg in productGroups)
            {
                SelectListItem option = new SelectListItem()
                {
                    Text = pg.Name + " - " + pg.Catalog.Name,
                    Value = pg.Id + "-" + pg.Catalog.Id
                };
                optionsProductGroups.Add(option);
            }
            ViewData["OptionsProductGroup"] = optionsProductGroups;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string productGroup, CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if(productGroup != null)
            {
                string[] par = productGroup.Split("-");
                int Id = Convert.ToInt32(par[0]);
                var result = await _productApiClient.CreateProduct(Id, request);
                if (result)
                {
                    TempData["Success"] = "Insert Success";
                    return RedirectToAction("", "product");
                }
            }
            TempData["Failure"] = "Let 's Choose Product Group";
            return View(request);
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> Update(int Id , int ProductGroup, UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            request.active = true;
            var result = await _productApiClient.UpdateProduct(Id , ProductGroup ,request);
            if (result != null)
            {
                TempData["Success"] = "Update Success";
                return RedirectToAction("detail","product",new { id =  Id});
            }
            TempData["Failure"] = "Update fail";
            return RedirectToAction("", "product");
        }

        [HttpPost]
        public async Task<IActionResult> Hide(int Id , int ProductGroup , UpdateProductRequest request)
        {
            var product = await _productApiClient.GetById(Id);
            request.name = product.Name;
            request.price = product.Price;
            request.unit = product.Unit;
            request.warranty = product.Warranty;
            request.description = product.Description;
            if (product.Active)
            {
                request.active = false;
            }
            else
            {
                request.active = true;
            }
            var result = await _productApiClient.UpdateProduct(Id ,ProductGroup,request);
            if (result != null)
            {
                TempData["Success"] = "Hide Success";
                return RedirectToAction("","product");
            }
            TempData["Failure"] = "Hide Fail";
            return RedirectToAction("","product");
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(int Id , AddItemRequest request)
        {
            var result = await _productApiClient.AddItemIntoProduct(Id , request);
            if (result)
            {
                TempData["Success"] = "Add Success";
                return RedirectToAction("detail", "product", new { id = Id });
            }
            TempData["Failure"] = "Add Fail";
            return RedirectToAction("", "product");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateItem(int Id , int ProductId , float SalePrice , float SellPrice , UpdateItemRequest request)
        {
            try
            {
                var item = await _productApiClient.GetItemById(Id);
                request.active = true;
                request.color = item.Color;
                request.name = item.Name;
                request.quantity = item.Quantity;
                request.salePrice = SalePrice;
                request.sellPrice = SellPrice;
                request.size = item.Size;
                
                var result = await _productApiClient.UpdateItemIntoProduct(Id, ProductId, request);
                TempData["Success"] = "Update Success";
                return RedirectToAction("detail", "product", new { id = ProductId });
            }
            catch(Exception e)
            {
                TempData["Failure"] = "Update Fail";
                return RedirectToAction("","error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertImage(int productid , int id , string url)
        {
            var result = await _productApiClient.InsertImageUrl(id, url);
            if (result)
            {
                TempData["Success"] = "Insert Success";
            }
            else
            {
                TempData["Failure"] = "Insert Failure";
            }
            return RedirectToAction("detail", "product" , new { id = productid});
        }
    }
}
public class UpdateProductRequest
{
    public string name { get; set; }
    public string description { get; set; }
    public float price { get; set; }
    public string unit { get; set; }
    public string warranty { get; set; }
    public bool active { get; set; }

}
public class AddItemRequest
{
    public string name { get; set; }
    public string color { get; set; }
    public string size { get; set; }
    public float sellPrice { get; set; }
    public float salePrice { get; set; }
    public bool active { get; set; }
    public int quantity { get; set; }


}
public class UpdateItemRequest
{
    public string name { get; set; }
    public string color { get; set; }
    public string size { get; set; }
    public float sellPrice { get; set; }
    public float salePrice { get; set; }
    public bool active { get; set; }
    public int quantity { get; set; }
}
public class CreateProductRequest
{
    public string name { get; set; }
    public string description { get; set; }
    public float price { get; set; }
    public string unit { get; set; }
    public string warranty { get; set; }
    public bool active { get; set; }
}