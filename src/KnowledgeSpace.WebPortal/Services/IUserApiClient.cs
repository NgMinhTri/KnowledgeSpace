using KnowledgeSpace.ViewModel.Systems;
using System.Threading.Tasks;

namespace KnowledgeSpace.WebPortal.Services
{
    public interface IUserApiClient
    {
        Task<GetUserVm> GetById(string id);
    }
}
