using KnowledgeSpace.ViewModel.Contents;
using Xunit;

namespace KnowledgeSpace.ViewModel.UnitTest.Contents
{
    public class VoteCreateRequestValidatorTest
    {
        private PostVoteVmValidator validator;
        private PostVoteVm request;

        public VoteCreateRequestValidatorTest()
        {
            request = new PostVoteVm()
            {
                KnowledgeBaseId = 1
            };
            validator = new PostVoteVmValidator();
        }

        [Fact]
        public void Should_Valid_Result_When_Valid_Request()
        {
            var result = validator.Validate(request);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Error_Result_When_KnowledgeBaseId_Is_Zero()
        {
            request.KnowledgeBaseId = 0;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }
    }
}