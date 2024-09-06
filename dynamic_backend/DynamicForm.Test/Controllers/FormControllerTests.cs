//using DynamicFormPresentation.Controllers;
//using DynamicFormServices.Dto;
//using DynamicFormServices.DynamicFormServiceInterface;
//using Moq;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;
//using Microsoft.AspNetCore.Mvc;
//using DynamicFormPresentation.Models;
//using DynamicFormService.DynamicFormServiceInterface;

//namespace DynamicForm.Test.Controllers
//{
//    public class FormControllerTests
//    {
//        private Mock<IDynamicFormServiceInterface> mockDynamicFormServiceInterface;
//        private Mock<IFormsService> mockFormsService;

//        public FormControllerTests()
//        {
//            this.mockDynamicFormServiceInterface = new Mock<IDynamicFormServiceInterface>();
//            this.mockFormsService = new Mock<IFormsService>();
//        }

//        private FormController CreateFormController()
//        {
//            return new FormController(
//                this.mockDynamicFormServiceInterface.Object,
//                this.mockFormsService.Object);
//        }

//        [Fact]
//        public async Task<FormDto> CreateForm_ValidForm_ReturnsCreatedResult()
//        {
//            // Arrange
//            var formController = this.CreateFormController();
//            var formDto = new FormDto
//            {
//                UserId = 1,
//                FormName = "Test Form",
//                Description = "Description",
//                IsPublish = true,
//                Version = 1
//            };

//            var formsTable = new FormsTable
//            {
//                Id = 1,
//                UserId = formDto.UserId.Value,
//                FormName = formDto.FormName,
//                Comments = formDto.Description,
//                IsPublish = formDto.IsPublish.Value,
//                Version = formDto.Version.Value,
//            };

//            mockDynamicFormServiceInterface
//                .Setup(s => s.CreateForm(It.IsAny<FormsTable>()))
//                .ReturnsAsync(formsTable);

//            // Act
//            var result = await formController.CreateForm(formDto);

//            // Assert
//            var createdResult = Assert.IsType<ObjectResult>(result);
//            Assert.Equal(201, createdResult.StatusCode);
//            Assert.Equal(formsTable.Id, createdResult.Value);
//        }

//        [Fact]
//        public async Task GetAllForms_ReturnsOkResultWithForms()
//        {
//            // Arrange
//            var formController = this.CreateFormController();
//            var formsList = new List<FormDto>
//            {
//                new FormDto { Id = 1, FormName = "Form1" },
//                new FormDto { Id = 2, FormName = "Form2" }
//            };

//            mockDynamicFormServiceInterface
//                .Setup(s => s.GetAllFormsAsync())
//                .ReturnsAsync(formsList);

//            // Act
//            var result = await formController.GetAllForms();

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(formsList, okResult.Value);
//        }

//        [Fact]
//        public async Task GetFormById_FormExists_ReturnsOkResultWithForm()
//        {
//            // Arrange
//            var formController = this.CreateFormController();
//            int formId = 1;
//            var formDto = new FormDto { Id = formId, FormName = "Form1" };

//            mockDynamicFormServiceInterface
//                .Setup(s => s.GetFormByIdAsync(formId))
//                .ReturnsAsync(formDto);

//            // Act
//            var result = await formController.GetFormById(formId);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(formDto, okResult.Value);
//        }

//        [Fact]
//        public async Task GetFormById_FormDoesNotExist_ReturnsNotFoundResult()
//        {
//            // Arrange
//            var formController = this.CreateFormController();
//            int formId = 1;

//            mockDynamicFormServiceInterface
//                .Setup(s => s.GetFormByIdAsync(formId))
//                .ReturnsAsync((FormDto)null);

//            // Act
//            var result = await formController.GetFormById(formId);

//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }

//        [Fact]
//        public async Task UpdateForm_ValidForm_ReturnsOkResultWithUpdatedForm()
//        {
//            // Arrange
//            var formController = this.CreateFormController();
//            int formId = 1;
//            var formDto = new FormDto
//            {
//                UserId = 1,
//                FormName = "Updated Form",
//                Description = "Updated Description",
//                IsPublish = true,
//                Version = 1
//            };

//            var updatedForm = new FormsTable
//            {
//                Id = formId,
//                UserId = formDto.UserId.Value,
//                FormName = formDto.FormName,
//                Comments = formDto.Description,
//                IsPublish = formDto.IsPublish.Value,
//                Version = formDto.Version.Value + 1
//            };

//            mockDynamicFormServiceInterface
//                .Setup(s => s.UpdateFormAsync(It.IsAny<FormsTable>()))
//                .ReturnsAsync(updatedForm);

//            // Act
//            var result = await formController.UpdateForm(formId, formDto);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(updatedForm, okResult.Value);
//        }

//        [Fact]
//        public async Task DeleteForm_FormExists_ReturnsNoContentResult()
//        {
//            // Arrange
//            var formController = this.CreateFormController();
//            int formId = 1;

//            mockDynamicFormServiceInterface
//                .Setup(s => s.DeleteFormAsync(formId))
//                .ReturnsAsync(true);

//            // Act
//            var result = await formController.DeleteForm(formId);

//            // Assert
//            Assert.IsType<NoContentResult>(result);
//        }

//        [Fact]
//        public async Task DeleteForm_FormDoesNotExist_ReturnsNotFoundResult()
//        {
//            // Arrange
//            var formController = this.CreateFormController();
//            int formId = 1;

//            mockDynamicFormServiceInterface
//                .Setup(s => s.DeleteFormAsync(formId))
//                .ReturnsAsync(false);

//            // Act
//            var result = await formController.DeleteForm(formId);

//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }

//        [Fact]
//        public async Task GetFormDetails_FormExists_ReturnsOkResultWithFormDetails()
//        {
//            // Arrange
//            var formController = this.CreateFormController();
//            int formId = 1;
//            var formDetailsDto = new FormDetailsDto { FormId = formId, Fields = new List<string> { "Field1", "Field2" } };

//            mockFormsService
//                .Setup(s => s.GetFormDetailsByFormIdAsync(formId))
//                .ReturnsAsync(formDetailsDto);

//            // Act
//            var result = await formController.GetFormDetails(formId);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            Assert.Equal(formDetailsDto, okResult.Value);
//        }

//        [Fact]
//        public async Task GetFormDetails_FormDoesNotExist_ReturnsNotFoundResult()
//        {
//            // Arrange
//            var formController = this.CreateFormController();
//            int formId = 1;

//            mockFormsService
//                .Setup(s => s.GetFormDetailsByFormIdAsync(formId))
//                .ReturnsAsync((FormDetailsDto)null);

//            // Act
//            var result = await formController.GetFormDetails(formId);

//            // Assert
//            Assert.IsType<NotFoundResult>(result);
//        }
//    }
//}
