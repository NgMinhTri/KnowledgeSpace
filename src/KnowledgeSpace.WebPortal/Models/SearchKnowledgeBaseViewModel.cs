using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Contents;

namespace KnowledgeSpace.WebPortal.Models
{
    public class SearchKnowledgeBaseViewModel
    {
        public Pagination<KnowledgeBaseQuickVm> Data { set; get; }

        public string Keyword { set; get; }
    }
}
