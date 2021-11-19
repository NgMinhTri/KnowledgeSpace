using KnowledgeSpace.ViewModel.Systems;
using Xunit;

namespace KnowledgeSpace.ViewModel.UnitTest.Systems
{
    public class FunctionCreateRequestValidatorTest
    {
        private PostFunctionVmValidator validator;
        private PostFunctionVm request;

        public FunctionCreateRequestValidatorTest()
        {
            request = new PostFunctionVm()
            {
                Id = "test6",
                ParentId = null,
                Name = "test6",
                SortOrder = 6,
                Url = "/test6"
            };
            validator = new PostFunctionVmValidator();
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
        public void Should_Error_Result_When_Miss_Id(string data)
        {
            request.Id = data;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Error_Result_When_Miss_Name(string data)
        {
            request.Name = data;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Error_Result_When_Miss_Url(string data)
        {
            request.Url = data;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }
    }
}