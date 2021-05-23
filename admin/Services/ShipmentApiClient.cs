using admin_webapp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using view_model.Catalog.Shipments;
using view_model.Common;

namespace admin_webapp.Services
{
    public class ShipmentApiClient : IShipmentApiClient
    {
        
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public ShipmentApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<ApiResult<PagedResult<ShipmentVm>>> GetPagings(GetShipmentPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainString"]);
            var response = await client.GetAsync("api/shipments/paging?" +
                $"pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Code}");
            var body = await response.Content.ReadAsStringAsync();
            var shipments = JsonConvert.DeserializeObject<ApiResult<PagedResult<ShipmentVm>>>(body);
            return shipments;
        }
    }
}
