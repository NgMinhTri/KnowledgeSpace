using FluentValidation;

namespace KnowledgeSpace.ViewModel.Contents
{
    public class PostCategoryVmValidator : AbstractValidator<PostCategoryVm>
    {
        public PostCategoryVmValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(Messages.Required, "Tên"));

            RuleFor(x => x.SeoAlias).NotEmpty().WithMessage(string.Format(Messages.Required, "Seo alias"));
        }
    }
}
