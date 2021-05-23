using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using view_model.Common;
using view_model.System.Languages;

namespace admin_webapp.Services
{
    public class LanguageApiClient : ILanguageApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public LanguageApiClient(IHttpClientFactory httpClientFactory , IHttpContextAccessor httpContextAccessor , IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<ApiResult<List<LanguageVm>>> GetAll()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["DomainString"]);
            var response = await client.GetAsync("/api/languages");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiResult<List<LanguageVm>>>(body);

            return JsonConvert.DeserializeObject<ApiResult<List<LanguageVm>>>(body);
        }
    }
}
