using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Contents;
using KnowledgeSpace.ViewModel.Systems;
using System.Threading.Tasks;

namespace KnowledgeSpace.WebPortal.Services
{
    public interface IUserApiClient
    {
        Task<GetUserVm> GetById(string id);

        Task<Pagination<KnowledgeBaseQuickVm>> GetKnowledgeBasesByUserId(string userId, int pageIndex, int pageSize);
    }
}
