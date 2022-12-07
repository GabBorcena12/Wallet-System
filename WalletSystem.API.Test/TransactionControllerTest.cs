using ExamWalletSystem.Controllers;
using ExamWalletSystem.Interface;
using ExamWalletSystem.Model;
using ExamWalletSystem.Model.Dto;
using ExamWalletSystem.Repository;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace WalletSystem.API.Test
{
    public class TransactionControllerTest
    {
        public TransactionControllerTest()
        {
        }
        [Fact]
        public async Task GetTransactionTest()
        {
            //Arrange 

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { 
                new Claim(ClaimTypes.NameIdentifier, "string test 123"), 
            }, "mock"));

            int count = 6;
            string userId = "string test 123";
            var fakeTransaction = A.CollectionOfDummy<TransactionDto>(count).ToList();
            var dataStore = A.Fake<ITransaction>();
            A.CallTo(() => dataStore.GetTransaction(userId)).Returns(Task.FromResult(fakeTransaction));

            var controller = new TransactionController(dataStore);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
             

            //Act
            var actionResult = await controller.GetUserTransaction();

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var returnTransaction = result.Value as IList<TransactionDto>;
            Assert.Equal(count, returnTransaction.Count);
        }

        [Fact]
        public async Task DepositTest()
        {
            // Arrange
            DepositDto testItem = new DepositDto()
            {
                AccountNumberTo = "126427770499",
                Amount = 1000
            };
            // Act

            int count = 1;
            var fakeTransaction = A.CollectionOfDummy<DepositDto>(count).AsEnumerable();
            var dataStore = A.Fake<ITransaction>(); 

            var controller = new TransactionController(dataStore);
            var createdResponse = controller.Deposit(testItem);
            // Assert
            await Assert.IsType<Task<IActionResult>>(createdResponse);
        }

        [Fact]
        public async Task WithdrawTest()
        {
            // Arrange
            WithdrawDto testItem = new WithdrawDto()
            {
                AccountNumberFrom = "126427770499",
                Amount = 1000
            };
            // Act

            int count = 1;
            var fakeTransaction = A.CollectionOfDummy<WithdrawDto>(count).AsEnumerable();
            var dataStore = A.Fake<ITransaction>();

            var controller = new TransactionController(dataStore);
            var createdResponse = controller.Withdraw(testItem);
            // Assert
            await Assert.IsType<Task<IActionResult>>(createdResponse);
        }

        [Fact]
        public async Task FundTransferTest()
        {
            // Arrange
            TransactDto testItem = new TransactDto()
            {
                AccountNumberTo = "126427770499",
                AccountNumberFrom = "126663766484",
                Amount = 1000
            };
            // Act

            int count = 1;
            var fakeTransaction = A.CollectionOfDummy<TransactDto>(count).AsEnumerable();
            var dataStore = A.Fake<ITransaction>();

            var controller = new TransactionController(dataStore);
            var createdResponse = controller.FundTransfer(testItem);
            // Assert
            await Assert.IsType<Task<IActionResult>>(createdResponse);
        }
    }
}
