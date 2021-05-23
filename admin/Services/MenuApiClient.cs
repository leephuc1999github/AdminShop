using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using view_model.Utilities.Menus;

namespace admin_webapp.Services
{
    public class MenuApiClient : IMenuApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public MenuApiClient(IHttpClientFactory httpClientFactory , IHttpContextAccessor httpContextAccessor , IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<List<MenuItemVm>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainString"]);
            var response = await client.GetAsync("/api/menus");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<MenuItemVm>>(body);

            return JsonConvert.DeserializeObject<List<MenuItemVm>>(body);
        }
    }
}
