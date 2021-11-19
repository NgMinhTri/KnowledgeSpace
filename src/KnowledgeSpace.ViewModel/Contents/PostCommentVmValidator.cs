using FluentValidation;

namespace KnowledgeSpace.ViewModel.Contents
{
    public class PostCommentVmValidator : AbstractValidator<PostCommentVm>
    {
        public PostCommentVmValidator()
        {
            RuleFor(x => x.KnowledgeBaseId).GreaterThan(0)
                .WithMessage("Mã bài đăng không đúng");

            RuleFor(x => x.Content).NotEmpty().WithMessage("Chưa nhập nội dung");

            RuleFor(x => x.CaptchaCode).NotEmpty()
              .WithMessage("Nhập mã xác nhận");
        }
    }
}
