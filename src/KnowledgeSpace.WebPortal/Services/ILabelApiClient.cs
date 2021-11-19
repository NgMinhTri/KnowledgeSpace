using KnowledgeSpace.ViewModel.Contents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeSpace.WebPortal.Services
{
    public interface ILabelApiClient
    {
        Task<List<LabelVm>> GetPopularLabels(int take);

        Task<LabelVm> GetLabelById(string labelId);
    }
}