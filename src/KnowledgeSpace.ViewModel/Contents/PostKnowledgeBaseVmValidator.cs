using FluentValidation;

namespace KnowledgeSpace.ViewModel.Contents
{
    public class PostKnowledgeBaseVmValidator : AbstractValidator<PostKnowledgeBaseVm>
    {
        public PostKnowledgeBaseVmValidator()
        {
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Category is required");

            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");

            RuleFor(x => x.Problem).NotEmpty().WithMessage("Problem is required");

            RuleFor(x => x.Note).NotEmpty().WithMessage("Note is required");
        }
    }
}
