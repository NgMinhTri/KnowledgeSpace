
using KnowledgeSpace.ViewModel.Contents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KnowledgeSpace.WebPortal.Services
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {

        public CategoryApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
           
        }

        public async Task<List<CategoryVm>> GetCategories()
        {
            //var client = _httpClientFactory.CreateClient();
            //client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            //var response = await client.GetAsync($"/api/categories");
            //var categories = JsonConvert.DeserializeObject<List<CategoryVm>>(await response.Content.ReadAsStringAsync());
            //return categories;
            return await GetListAsync<CategoryVm>("/api/categories");

        }

        public async Task<CategoryVm> GetCategoryById(int id)
        {
            return await GetAsync<CategoryVm>($"/api/categories/{id}");
        }
    }
}