using KnowledgeSpace.ViewModel.Contents;
using Xunit;

namespace KnowledgeSpace.ViewModel.UnitTest.Contents
{
    public class CategoryCreateRequestValidatorTest
    {
        private PostCategoryVmValidator validator;
        private PostCategoryVm request;

        public CategoryCreateRequestValidatorTest()
        {
            request = new PostCategoryVm()
            {
                Name = "test",
                ParentId = null,
                SeoAlias = "test",
                SeoDescription = "test",
                SortOrder = 1
            };
            validator = new PostCategoryVmValidator();
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

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Error_Result_When_Miss_SeoAlias(string seoAlias)
        {
            request.SeoAlias = seoAlias;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }
    }
}