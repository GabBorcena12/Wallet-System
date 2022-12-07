using ExamWalletSystem.Controllers;
using ExamWalletSystem.Interface;
using ExamWalletSystem.Model.Dto;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WalletSystem.API.Test
{
    public class AccountControllerTest
    {
        [Fact]
        public async Task RegisterTest()
        {
            // Arrange
            RegisterUserDto testItem = new RegisterUserDto()
            {
                UserName = "SampleUser123",
                Password = "Admin@123"
            };
            // Act

            int count = 1;
            var fakeTransaction = A.CollectionOfDummy<RegisterUserDto>(count);
            var dataStore = A.Fake<IAccount>();

            var controller = new AccountController(dataStore);
            var createdResponse = controller.Register(testItem);
            // Assert
            await Assert.IsType<Task<ActionResult>>(createdResponse);
        }
        [Fact]
        public async Task LoginTest()
        {
            // Arrange
            RegisterUserDto testItem = new RegisterUserDto()
            {
                UserName = "SampleUser123",
                Password = "Admin@123"
            };
            // Act

            int count = 1;
            var fakeTransaction = A.CollectionOfDummy<RegisterUserDto>(count);
            var dataStore = A.Fake<IAccount>();

            var controller = new AccountController(dataStore);
            var createdResponse = controller.login(testItem);
            // Assert
            await Assert.IsType<Task<ActionResult>>(createdResponse);
        }
    }
}
