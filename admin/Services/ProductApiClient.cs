using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using view_model.Catalog.Categories;
using view_model.Catalog.Products;
using view_model.Common;

namespace admin_webapp.Services
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public ProductApiClient(IHttpClientFactory httpClientFactory , IHttpContextAccessor httpContextAccessor , IConfiguration configuration) 
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<bool> CreateProduct(int productGroup , CreateProductRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/products?productGroupId={productGroup}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return true;
            }
            return false;
        }

        public async Task<ApiResult<bool>> DeleteProduct(int productId)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainString"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.DeleteAsync($"api/products/{productId}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return new ApiSuccessResult<bool>();
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);

        }

        public async Task<ProductResponse> GetById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var response = await client.GetAsync($"api/products/{id}?fbclid=IwAR2-The3oSc_-FC4RtTHsIxm9J9KmUcv3HLdy0rBxMcEQxE6t6skLor_18U");
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProductResponse>(body);
        }

        public async Task<List<ProductResponse>> GetPagings(GetManageProductPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var response = await client.GetAsync("api/products");
            
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<ProductResponse>>(body);
                return products;
            }
            return null;
        }

        public async Task<ProductResponse> UpdateProduct(int id , int productGroup ,UpdateProductRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"api/products/{id}?productGroupId={productGroup}", httpContent);
            
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductResponse>(body);
            }
            return null;
        }

        public async Task<ApiResult<bool>> CategoryAssign(int id, CategoryAssignRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainString"]);
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/products/{id}/categories", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<bool> AddItemIntoProduct(int id , AddItemRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/items?productId={id}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return true;
            }
            return false;
        }
        public async Task<List<ProductGroupResponse>> GetAllProductGroup()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var response = await client.GetAsync("api/productGroups");

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var productGroups = JsonConvert.DeserializeObject<List<ProductGroupResponse>>(body);
                return productGroups;
            }
            return null;
        }

        public async Task<bool> UpdateItemIntoProduct(int id, int productId, UpdateItemRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"api/items/{id}?productId={productId}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return true;
            }
            return false;
        }

        public async Task<Item> GetItemById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var response = await client.GetAsync($"api/items/{id}");

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var item = JsonConvert.DeserializeObject<Item>(body);
                return item;
            }
            return null;
        }

        public async Task<bool> InsertImageUrl(int id, string url)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var json = JsonConvert.SerializeObject(new Image() { url = url});
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/itemimages?itemId={id}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return true;
            }
            return false;
        }
    }
}
public class Image
{
    public string url { get; set; }
}
public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public string Unit { get; set; }
    public bool Active { get; set; }
    public string Warranty { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    public ProductGroupResponse ProductGroup { get; set; }
    public List<Item> Items { get; set; }

}
public class ProductGroupResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    public CatalogResponse Catalog { get; set; }
}
public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public string Size { get; set; }
    public float SellPrice { get; set; }
    public float SalePrice { get; set; }
    public int Quantity { get; set; }
    public bool Active { get; set; }
    public string CreatedAt { get; set; }
    public string UpdateAt { get; set; }
    public List<ItemImage> ItemImages { get; set; }
}
public class ItemImage
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string CreatedAt { get; set; }
    public string UpdateAt { get; set; }
}