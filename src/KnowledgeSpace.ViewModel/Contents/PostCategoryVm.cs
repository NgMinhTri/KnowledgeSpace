namespace KnowledgeSpace.ViewModel.Contents
{
    public class PostCategoryVm
    {
        public string Name { get; set; }
        public string SeoAlias { get; set; }
        public string SeoDescription { get; set; }
        public int SortOrder { get; set; }
        public int? ParentId { get; set; }
    }
}
