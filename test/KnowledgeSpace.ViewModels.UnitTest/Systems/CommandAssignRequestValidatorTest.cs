using KnowledgeSpace.ViewModel.Systems;
using Xunit;

namespace KnowledgeSpace.ViewModel.UnitTest.Systems
{
    public class CommandAssignRequestValidatorTest
    {
        private PostCommandAssignVmValidator validator;
        private PostCommandAssignVm request;

        public CommandAssignRequestValidatorTest()
        {
            request = new PostCommandAssignVm()
            {
                AddToAllFunctions = true,
                CommandIds = new string[] { "ADD" }
            };
            validator = new PostCommandAssignVmValidator();
        }

        [Fact]
        public void Should_Valid_Result_When_Valid_Request()
        {
            var result = validator.Validate(request);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Should_Error_Result_When_Request_Miss_CommandIds()
        {
            request.CommandIds = null;
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Should_Error_Result_When_Request_CommandIds_Empty()
        {
            request.CommandIds = new string[] { };
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Should_Error_Result_When_Request_CommandIds_Contains_Any_Empty_Item()
        {
            request.CommandIds = new string[] { "" };
            var result = validator.Validate(request);
            Assert.False(result.IsValid);
        }
    }
}