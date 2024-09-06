using DynamicFormPresentation.Controllers;
using DynamicFormServices.DynamicFormServiceInterface;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DynamicForm.Test.Controllers
{
    public class AnswerOptionControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IAnswerOptionService> mockAnswerOptionService;

        public AnswerOptionControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockAnswerOptionService = this.mockRepository.Create<IAnswerOptionService>();
        }

        private AnswerOptionController CreateAnswerOptionController()
        {
            return new AnswerOptionController(
                this.mockAnswerOptionService.Object);
        }

        [Fact]
        public async Task GetById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var answerOptionController = this.CreateAnswerOptionController();
            int id = 0;

            // Act
            var result = await answerOptionController.GetById(
                id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
