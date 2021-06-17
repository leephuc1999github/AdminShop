using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using view_model.Catalog.Categories;

namespace admin_webapp.Services
{
    public class CategoryApiClient : ICategoryApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public CategoryApiClient(IHttpClientFactory httpClientFactory , IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<List<CatalogResponse>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainStringMe"]);
            var response = await client.GetAsync($"api/catalogs?fbclid=IwAR2v5mBn1Iz-ScB-d1YoQnmzK9nQ06azk-gm2uqxCm2jWLcdvLjJchwKdbA");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<CatalogResponse>>(body);

            return JsonConvert.DeserializeObject<List<CatalogResponse>>(body);
        }
    }
}

public class CatalogResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
}
