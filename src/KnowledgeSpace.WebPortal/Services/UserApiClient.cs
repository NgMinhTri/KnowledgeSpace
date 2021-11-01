using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Contents;
using KnowledgeSpace.ViewModel.Systems;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace KnowledgeSpace.WebPortal.Services
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        public UserApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
            : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<GetUserVm> GetById(string id)
        {
            return await GetAsync<GetUserVm>($"/api/users/{id}", true);
        }

        public async Task<Pagination<KnowledgeBaseQuickVm>> GetKnowledgeBasesByUserId(string userId, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/users/{userId}/knowledgeBases?pageIndex={pageIndex}&pageSize={pageSize}";
            return await GetAsync<Pagination<KnowledgeBaseQuickVm>>(apiUrl, true);
        }
    }
}