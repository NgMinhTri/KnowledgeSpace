using KnowledgeSpace.ViewModel;
using KnowledgeSpace.ViewModel.Contents;

namespace KnowledgeSpace.WebPortal.Models
{
    public class ListByTagIdViewModel
    {
        public Pagination<KnowledgeBaseQuickVm> Data { set; get; }

        public LabelVm LabelVm { set; get; }
    }
}