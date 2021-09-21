﻿using KnowledgeSpace.ViewModel;
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
    public class KnowledgeBaseApiClient : IKnowledgeBaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KnowledgeBaseApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<KnowledgeBaseQuickVm>> GetPopularKnowledgeBases(int take)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            var response = await client.GetAsync($"/api/knowledgeBases/popular/{take}");
            var latestKnowledgeBases = JsonConvert.DeserializeObject<List<KnowledgeBaseQuickVm>>(await response.Content.ReadAsStringAsync());
            return latestKnowledgeBases;
        }

        public async Task<List<KnowledgeBaseQuickVm>> GetLatestKnowledgeBases(int take)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            var response = await client.GetAsync($"/api/knowledgeBases/latest/{take}");
            var latestKnowledgeBases = JsonConvert.DeserializeObject<List<KnowledgeBaseQuickVm>>(await response.Content.ReadAsStringAsync());
            return latestKnowledgeBases;
        }


        public async Task<List<LabelVm>> GetPopularLabels(int take)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            var response = await client.GetAsync($"/api/labels/popular/{take}");
            var popularlabel = JsonConvert.DeserializeObject<List<LabelVm>>(await response.Content.ReadAsStringAsync());
            return popularlabel;
        }

        public async Task<Pagination<KnowledgeBaseQuickVm>> GetKnowledgeBasesByCategoryId(int categoryId, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/knowledgeBases/filter?categoryId={categoryId}&pageIndex={pageIndex}&pageSize={pageSize}";
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            var response = await client.GetAsync(apiUrl);
            var knowledgeBases = JsonConvert.DeserializeObject<Pagination<KnowledgeBaseQuickVm>>(await response.Content.ReadAsStringAsync());
            return knowledgeBases;
        }

        //Hàm lấy ra chi tiết KnowledgeBase
        public async Task<KnowledgeBaseVm> GetKnowledgeBaseDetail(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            var response = await client.GetAsync($"/api/knowledgeBases/{id}");
            var knowledgeBase = JsonConvert.DeserializeObject<KnowledgeBaseVm>(await response.Content.ReadAsStringAsync());
            return knowledgeBase; 
        }


        public async Task<List<LabelVm>> GetLabelsByKnowledgeBaseId(int id)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            var response = await client.GetAsync($"/api/knowledgeBases/{id}/labels");
            var labels = JsonConvert.DeserializeObject<List<LabelVm>>(await response.Content.ReadAsStringAsync());
            return labels;
        }

        //Tìm kiếm bài viết theo từ khóa
        public async Task<Pagination<KnowledgeBaseQuickVm>> SearchKnowledgeBase(string keyword, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/knowledgeBases/filter?filter={keyword}&pageIndex={pageIndex}&pageSize={pageSize}";
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            var response = await client.GetAsync(apiUrl);
            var knowledgeBases = JsonConvert.DeserializeObject<Pagination<KnowledgeBaseQuickVm>>(await response.Content.ReadAsStringAsync());
            return knowledgeBases;
        }
    }
}