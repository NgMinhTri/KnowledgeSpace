namespace KnowledgeSpace.ViewModel.Contents
{
    public class PostCommentVm
    {
        public string Content { get; set; }
        public int KnowledgeBaseId { get; set; }
        public int? ReplyId { get; set; }
        public string CaptchaCode { get; set; }
    }
}
