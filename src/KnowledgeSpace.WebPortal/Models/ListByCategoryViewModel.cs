using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Contents;

namespace KnowledgeSpace.WebPortal.Models
{
    public class ListByCategoryViewModel
    {
        public  Pagination<KnowledgeBaseQuickVm> Data { get; set; }
        public CategoryVm Category { get; set; }
    }
}
