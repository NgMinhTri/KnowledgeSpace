using KnowledgeSpace.ViewModel.Contents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeSpace.WebPortal.Services
{
    public interface ICategoryApiClient
    {
        Task<List<CategoryVm>> GetCategories();
    }
}
