using KnowledgeSpace.ViewModel.Contents;
using Xunit;

namespace KnowledgeSpace.ViewModel.UnitTest.Contents
{
    public class LabelCreateRequestValidatorTest
    {
        private PostLabelVmValidator validator;
        private PostLabelVm request;

        public LabelCreateRequestValidatorTest()
        {
            request = new PostLabelVm()
            {
                Name = "test"
            };
            validator = new PostLabelVmValidator();
        }

        [Fact]
        public void Should_Valid_Result_When_Valid_Request()
        {
            var result = validator.Validate(request);
            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Error_Result_When_Miss_Name(string name)
        {
            request.Name = name;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }
    }
}