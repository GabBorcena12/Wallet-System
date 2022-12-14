using ExamWalletSystem.Controllers;
using ExamWalletSystem.Interface;
using ExamWalletSystem.Model;
using ExamWalletSystem.Model.Dto;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace WalletSystem.API.Test
{
    public class TransactionControllerTest
    {  
        private readonly ClaimsPrincipal _currentUser;  
        private readonly TransactDto _transactDto;
        private readonly WithdrawDto _withdrawDto;
        private readonly DepositDto _depositDto;
        private string _userId = "Admin";
        public TransactionControllerTest()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "Admin"),
            }, "User"));  

            _transactDto = new TransactDto()
            {
                AccountNumberTo = 121249488198,
                AccountNumberFrom = 121271789724,
                Amount = 1000
            };

            _withdrawDto = new WithdrawDto()
            {
                AccountNumberFrom = 121249488198,
                Amount = 1000
            };

            _depositDto = new DepositDto()
            {
                AccountNumberTo = 121249488198,
                Amount = 1000
            }; 
        }
        
        [Fact]
        public async Task GetAllTransaction()
        {
            //Arrange
            int count = 3;
            var fakeTransaction = A.CollectionOfDummy<TransactionDto>(count).ToList();
            var dataStore = A.Fake<ITransaction>();
            A.CallTo(() => dataStore.GetTransaction(_userId)).Returns(Task.FromResult(fakeTransaction));

            var controller = new TransactionController(dataStore);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _currentUser }
            }; 
 
            //Act
            var actionResult = await controller.GetUserTransaction();

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var returnTransaction = result.Value as IList<TransactionDto>;
            Assert.Equal(count, returnTransaction.Count);
            Assert.NotNull(actionResult); 
        }

        [Fact]
        public async Task GetAllTransaction_ReturnNoRows()
        {
            //Arrange
            int count = 0;
            string userId = "Guest";
            var fakeTransaction = A.CollectionOfDummy<TransactionDto>(count).ToList();
            var dataStore = A.Fake<ITransaction>();
            A.CallTo(() => dataStore.GetTransaction(userId)).Returns(Task.FromResult(fakeTransaction));

            var controller = new TransactionController(dataStore);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = _currentUser }
            };

            //Act
            var actionResult = await controller.GetUserTransaction();

            //Assert
            var result = actionResult.Result as OkObjectResult;
            var returnTransaction = result.Value as IList<TransactionDto>;
            Assert.Equal(count, returnTransaction.Count);
        }


        [Fact]
        public async Task Deposit_TestCase001()
        {
            //Arrange 
            var fakeTransaction = A.CollectionOfDummy<DepositDto>(1).AsEnumerable();
            var dataStore = A.Fake<ITransaction>();

            // Act 
            var controller = new TransactionController(dataStore);
            var createdResponse = controller.Deposit(_depositDto);
            // Assert 
            await Assert.IsType<Task<IActionResult>>(createdResponse);
            Assert.NotNull(createdResponse);
        }
         
        [Fact]
        public async Task Deposit_TestCase002()
        {
            // Arrange & Act
            var mockRepo = new Mock<ITransaction>();
            var controller = new TransactionController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Deposit(model: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); 
        }

        [Fact]
        public async Task Deposit_TestCase003()
        {
            // Arrange & Act
            DepositDto depositDto = new DepositDto()
            {
                AccountNumberTo = 121249488198,
                Amount = 0
            };

            var mockRepo = new Mock<ITransaction>();
            var controller = new TransactionController(mockRepo.Object); 

            // Act
            var result = await controller.Deposit(model: depositDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task Deposit_TestCase004()
        {
            // Arrange & Act
            DepositDto depositDto = new DepositDto()
            {
                AccountNumberTo = 0,
                Amount = 1000
            };

            var mockRepo = new Mock<ITransaction>();
            var controller = new TransactionController(mockRepo.Object);

            // Act
            var result = await controller.Deposit(model: depositDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task Deposit_TestCase005()
        {
            // Arrange & Act
            DepositDto depositDto = new DepositDto()
            {
                AccountNumberTo = 0,
                Amount = 0
            };

            var mockRepo = new Mock<ITransaction>();
            var controller = new TransactionController(mockRepo.Object);

            // Act
            var result = await controller.Deposit(model: depositDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }


        [Fact]
        public async Task Withdraw_TestCase001()
        {
            //Arrange
            var fakeTransaction = A.CollectionOfDummy<WithdrawDto>(1).AsEnumerable();
            var dataStore = A.Fake<ITransaction>();

            // Act 
            var controller = new TransactionController(dataStore);
            var createdResponse = controller.Withdraw(_withdrawDto);
            // Assert 
            await Assert.IsType<Task<IActionResult>>(createdResponse);
            Assert.NotNull(createdResponse);
        }

        [Fact]
        public async Task Withdraw_TestCase002()
        {
            // Arrange & Act
            var mockRepo = new Mock<ITransaction>();
            var controller = new TransactionController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Withdraw(model: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Withdraw_TestCase003()
        {
            // Arrange & Act
            WithdrawDto withdrawDto = new WithdrawDto()
            {
                AccountNumberFrom = 121249488198,
                Amount = 0
            };
            var mockRepo = new Mock<ITransaction>();
            var controller = new TransactionController(mockRepo.Object); 

            // Act
            var result = await controller.Withdraw(model: withdrawDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task Withdraw_TestCase004()
        {
            // Arrange & Act
            WithdrawDto withdrawDto = new WithdrawDto()
            {
                AccountNumberFrom = 0,
                Amount = 1000
            };
            var mockRepo = new Mock<ITransaction>();
            var controller = new TransactionController(mockRepo.Object);

            // Act
            var result = await controller.Withdraw(model: withdrawDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }
        [Fact]
        public async Task Withdraw_TestCase005()
        {
            // Arrange & Act
            WithdrawDto withdrawDto = new WithdrawDto()
            {
                AccountNumberFrom = 0,
                Amount = 0
            };
            var mockRepo = new Mock<ITransaction>();
            var controller = new TransactionController(mockRepo.Object);

            // Act
            var result = await controller.Withdraw(model: withdrawDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task FundTransfer_TestCase001()
        {
            //Arrange
            var fakeTransaction = A.CollectionOfDummy<TransactDto>(1).AsEnumerable();
            var dataStore = A.Fake<ITransaction>();

            // Act 
            var controller = new TransactionController(dataStore);
            var createdResponse = controller.FundTransfer(_transactDto);
            // Assert 
            await Assert.IsType<Task<IActionResult>>(createdResponse);
            Assert.NotNull(createdResponse);
        }

        [Fact]
        public async Task FundTransfer_TestCase002()
        {
            // Arrange & Act
            var mockRepo = new Mock<ITransaction>();
            var controller = new TransactionController(mockRepo.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.FundTransfer(model: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task FundTransfer_TestCase003()
        {
            // Arrange & Act
            TransactDto transactDto = new TransactDto()
            {
                AccountNumberTo = 121249488198,
                AccountNumberFrom = 121271789724,
                Amount = 0
            };
            var mockRepo = new Mock<ITransaction>();
            var controller = new TransactionController(mockRepo.Object); 

            // Act
            var result = await controller.FundTransfer(model: transactDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task FundTransfer_TestCase004()
        {
            // Arrange & Act
            TransactDto transactDto = new TransactDto()
            {
                AccountNumberTo = 0,
                AccountNumberFrom = 0,
                Amount = 1000
            };
            var mockRepo = new Mock<ITransaction>();
            var controller = new TransactionController(mockRepo.Object);

            // Act
            var result = await controller.FundTransfer(model: transactDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task FundTransfer_TestCase006()
        {
            // Arrange & Act
            var mockRepo = new Mock<ITransaction>();
            var controller = new TransactionController(mockRepo.Object); 

            // Act
            var result = await controller.FundTransfer(model: null);

            // Assert
            Assert.IsType<ObjectResult>(result);
        }

        private List<TransactionDto> GetTestSessions()
        {
            var sessions = new List<TransactionDto>();
            sessions.Add(new TransactionDto()
            {
                AccountNumberFrom = 121249488198,
                AccountNumberTo = 121271789724,
                TransactionType = "Fund Transfer",
                Amount = 100,
                EndBalance = 1000,
                DateOfTransaction = new DateTime(2016, 7, 2) 
            });
            sessions.Add(new TransactionDto()
            {
                AccountNumberFrom = 121249488198,
                AccountNumberTo = 121271789724,
                TransactionType = "Fund Transfer",
                Amount = 200,
                EndBalance = 1000,
                DateOfTransaction = new DateTime(2016, 8, 2)
            });
            return sessions;
        }
    }
}
