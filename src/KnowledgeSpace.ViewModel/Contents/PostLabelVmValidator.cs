using FluentValidation;

namespace KnowledgeSpace.ViewModel.Contents
{
    public class PostLabelVmValidator : AbstractValidator<PostLabelVm>
    {
        public PostLabelVmValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(Messages.Required, "Tên"));
        }
    }
}
