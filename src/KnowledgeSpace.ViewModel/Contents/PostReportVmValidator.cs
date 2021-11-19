using FluentValidation;

namespace KnowledgeSpace.ViewModel.Contents
{
    public class PostReportVmValidator : AbstractValidator<PostReportVm>
    {
        public PostReportVmValidator()
        {
            RuleFor(x => x.Content).NotEmpty().WithMessage("Phải nhập nội dung");

            RuleFor(x => x.KnowledgeBaseId).NotNull().WithMessage("Chưa có mã bài đăng");

            RuleFor(x => x.CaptchaCode).NotEmpty().WithMessage("Bạn chưa nhập mã xác nhận");
        }
    }
}
