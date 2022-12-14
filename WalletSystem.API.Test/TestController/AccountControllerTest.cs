using ExamWalletSystem.Controllers;
using ExamWalletSystem.Interface;
using ExamWalletSystem.Model.Dto;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WalletSystem.API.Test.TestController
{
    public class AccountControllerTest
    {
        private readonly RegisterUserDto _userDto; 
        public AccountControllerTest()
        {
            _userDto = new RegisterUserDto()
            {
                UserName = "Admin",
                Password = "Admin@123"
            };
             
        }

        [Fact]
        public async Task Register_TestCase001()
        { 
            int count = 1;
            var fakeTransaction = A.CollectionOfDummy<RegisterUserDto>(count);
            var dataStore = A.Fake<IAccount>();

            var controller = new AccountController(dataStore);
            var createdResponse = controller.Register(_userDto); 
            await Assert.IsType<Task<ActionResult>>(createdResponse);
        }

        [Fact]
        public async Task Register_TestCase002()
        {

            // Arrange & Act
            var mockRepo = new Mock<IAccount>();
            var controller = new AccountController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Register(_userDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); 
        }

        [Fact]
        public async Task Register_TestCase003()
        {
            // Arrange & Act
            RegisterUserDto userDto = new RegisterUserDto()
            {
                UserName = "",
                Password = ""
            };
            var mockRepo = new Mock<IAccount>();
            var controller = new AccountController(mockRepo.Object);

            // Act
            var result = await controller.Register(userDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Login_TestCase001()
        {  
            int count = 1;
            var fakeTransaction = A.CollectionOfDummy<RegisterUserDto>(count);
            var dataStore = A.Fake<IAccount>();

            var controller = new AccountController(dataStore);
            var createdResponse = controller.login(_userDto); 
            await Assert.IsType<Task<ActionResult>>(createdResponse);
        }

        [Fact]
        public async Task Login_TestCase002()
        {

            // Arrange & Act
            var mockRepo = new Mock<IAccount>();
            var controller = new AccountController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.login(_userDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }


        [Fact]
        public async Task Login_TestCase003()
        {
            // Arrange & Act
            RegisterUserDto userDto = new RegisterUserDto()
            {
                UserName = "",
                Password = ""
            };
            var mockRepo = new Mock<IAccount>();
            var controller = new AccountController(mockRepo.Object);

            // Act
            var result = await controller.login(userDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }
    }
}
