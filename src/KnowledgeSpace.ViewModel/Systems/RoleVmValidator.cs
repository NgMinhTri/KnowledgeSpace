using FluentValidation;

namespace KnowledgeSpace.ViewModel.Systems
{
    public class RoleVmValidator : AbstractValidator<RoleVm>
    {
        public RoleVmValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required")
                .MaximumLength(50).WithMessage("Role id cannot over limit 50 characters");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Role Name is required");
        }
    }
}
