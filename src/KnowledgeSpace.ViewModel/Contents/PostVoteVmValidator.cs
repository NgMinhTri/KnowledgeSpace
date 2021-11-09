using FluentValidation;

namespace KnowledgeSpace.ViewModel.Contents
{
    public class PostVoteVmValidator : AbstractValidator<PostVoteVm>
    {
        public PostVoteVmValidator()
        {
            RuleFor(x => x.KnowledgeBaseId)
               .GreaterThan(0)
               .WithMessage(string.Format(Messages.Required, "Mã bài đăng"));
        }
    }
}
