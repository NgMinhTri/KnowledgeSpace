using KnowledgeSpace.ViewModel.Contents;
using KnowledgeSpace.ViewModel.Systems;
using System.Collections.Generic;

namespace KnowledgeSpace.WebPortal.Models
{
    public class KnowledgeBaseDetailViewModel
    {
        public CategoryVm Category { set; get; }
        public KnowledgeBaseVm Detail { get; set; }
        public List<LabelVm> Labels { get; set; }
        public GetUserVm CurrentUser { get; set; }
    }
}
