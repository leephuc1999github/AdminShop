using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using view_model.Common;

namespace admin.Services
{
    public interface IOrderApiClient
    {
        Task<List<OrderResponse>> GetPagings(GetOrderPagingRequest request);
        Task<bool> UpdateStatus(int id, string note, string status);
        Task<OrderResponse> GetOrderById(int id);
    }
    public class OrderApiClient : IOrderApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public OrderApiClient(IHttpClientFactory httpClientFactory , IHttpContextAccessor httpContextAccessor , IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OrderResponse> GetOrderById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var response = await client.GetAsync($"api/orders/{id}");

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OrderResponse>(body);
            }
            return null;
        }

        public async Task<List<OrderResponse>> GetPagings(GetOrderPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var response = await client.GetAsync("api/orders");
            
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<OrderResponse>>(body);
            }
            return null;
        }

        public async Task<bool> UpdateStatus(int id, string note, string status)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var json = JsonConvert.SerializeObject(new StatusOrder() { status = status , note = note});
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/orders/{id}",httpContent);

            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return true;
            }
            return false;
        }
    }
}
public class StatusOrder
{
    public string note { get; set; }
    public string status { get; set; }
}
public class OrderResponse
{
    public int Id { get; set; }
    public string RecipientName { get; set; }
    public string RecipientPhoneNumber { get; set; }
    public string RecipientEmail { get; set; }
    public string DetailedAddress { get; set; }
    public string Ward { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Note { get; set; }
    public string Status { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    public Cart Cart { get; set; }
    public Payment Payment { get; set; }
}
public class Cart
{
    public int Id { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    public bool Using { get; set; }
    public List<ItemCart> Cart_Items { get; set; }
}
public class ItemCart
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public Item Item { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
}
public class Payment
{
    public int Id { get; set; }
    public string PaymentMethod { get; set; }
    public bool Paid { get; set; }
    public float TotalPayment { get; set; }
    public float Discount { get; set; }
    public float PrePayment { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    public Cart Cart { get; set; }
}
public class GetOrderPagingRequest : PagingRequestBase
{

}
