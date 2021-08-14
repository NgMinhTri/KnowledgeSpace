using FluentValidation;

namespace KnowledgeSpace.ViewModel.Contents
{
    public class PostCommentVmValidator : AbstractValidator<PostCommentVm>
    {
        public PostCommentVmValidator()
        {
            RuleFor(x => x.KnowledgeBaseId).GreaterThan(0)
                .WithMessage("Knowledge base Id is not valid");

            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required");
        }
    }
}
