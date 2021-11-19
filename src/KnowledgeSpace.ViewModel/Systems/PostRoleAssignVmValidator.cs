using FluentValidation;

namespace KnowledgeSpace.ViewModel.Systems
{
    public class PostRoleAssignVmValidator : AbstractValidator<PostRoleAssignVm>
    {
        public PostRoleAssignVmValidator()
        {
            RuleFor(x => x.RoleNames).NotNull()
                .WithMessage(string.Format(Messages.Required, "Tên quyền"));

            RuleFor(x => x.RoleNames).Must(x => x.Length > 0)
                .When(x => x.RoleNames != null)
             .WithMessage(string.Format(Messages.Required, "Tên quyền"));

            RuleForEach(x => x.RoleNames).NotEmpty()
                .WithMessage(string.Format(Messages.Required, "Tên quyền"));
        }
    }
}
