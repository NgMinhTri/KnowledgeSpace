using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Contents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KnowledgeSpace.WebPortal.Services
{
    public class KnowledgeBaseApiClient : BaseApiClient, IKnowledgeBaseApiClient
    {

        public KnowledgeBaseApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor): base(httpClientFactory, configuration, httpContextAccessor)
        {
           
        }

        public async Task<List<KnowledgeBaseQuickVm>> GetPopularKnowledgeBases(int take)
        {
            //var client = _httpClientFactory.CreateClient();
            //client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            //var response = await client.GetAsync($"/api/knowledgeBases/popular/{take}");
            //var latestKnowledgeBases = JsonConvert.DeserializeObject<List<KnowledgeBaseQuickVm>>(await response.Content.ReadAsStringAsync());
            //return latestKnowledgeBases;
            return await GetListAsync<KnowledgeBaseQuickVm>($"/api/knowledgeBases/popular/{take}");
        }

        public async Task<List<KnowledgeBaseQuickVm>> GetLatestKnowledgeBases(int take)
        {
            return await GetListAsync<KnowledgeBaseQuickVm>($"/api/knowledgeBases/latest/{take}");
        }

        public async Task<Pagination<KnowledgeBaseQuickVm>> GetKnowledgeBasesByCategoryId(int categoryId, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/knowledgeBases/filter?categoryId={categoryId}&pageIndex={pageIndex}&pageSize={pageSize}";
            return await GetAsync<Pagination<KnowledgeBaseQuickVm>>(apiUrl);
        }

        //Hàm lấy ra chi tiết KnowledgeBase
        public async Task<KnowledgeBaseVm> GetKnowledgeBaseDetail(int id)
        {
            return await GetAsync<KnowledgeBaseVm>($"/api/knowledgeBases/{id}");
        }


        public async Task<List<LabelVm>> GetLabelsByKnowledgeBaseId(int id)
        {
            return await GetListAsync<LabelVm>($"/api/knowledgeBases/{id}/labels");
        }

        //Tìm kiếm bài viết theo từ khóa
        public async Task<Pagination<KnowledgeBaseQuickVm>> SearchKnowledgeBase(string keyword, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/knowledgeBases/filter?filter={keyword}&pageIndex={pageIndex}&pageSize={pageSize}";
            return await GetAsync<Pagination<KnowledgeBaseQuickVm>>(apiUrl);
        }

        public async Task<Pagination<KnowledgeBaseQuickVm>> GetKnowledgeBasesByTagId(string tagId, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/knowledgeBases/tags/{tagId}?pageIndex={pageIndex}&pageSize={pageSize}";
            return await GetAsync<Pagination<KnowledgeBaseQuickVm>>(apiUrl);
        }

        public async Task<List<CommentVm>> GetRecentComments(int take)
        {
            return await GetListAsync<CommentVm>($"/api/knowledgeBases/comments/recent/{take}");
        }
    }
}
