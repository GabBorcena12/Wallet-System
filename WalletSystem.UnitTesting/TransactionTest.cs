using NUnit.Framework;  
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using ExamWalletSystem.Model.Dto;
using ExamWalletSystem.Controllers;

namespace WalletSystem.UnitTesting
{
    public class TransactionTest
    {
        [SetUp]
        public void Setup()
        {
            TransactionDto aFund = new TransactionDto() {
                AccountNumberFrom = 121249488198,
                AccountNumberTo = 121271789724,
                TransactionType = "Fund Transfer",
                Amount = 100,
                EndBalance = 1000,
                DateOfTransaction = new DateTime(2016, 7, 2)
            };
            TransactionDto aFund1 = new TransactionDto()
            {
                AccountNumberFrom = 121249488198,
                AccountNumberTo = 121271789724,
                TransactionType = "Fund Transfer",
                Amount = 300,
                EndBalance = 1000,
                DateOfTransaction = new DateTime(2016, 7, 2)
            };
            TransactionDto aFund2 = new TransactionDto()
            {
                AccountNumberFrom = 121249488198,
                AccountNumberTo = 121271789724,
                TransactionType = "Fund Transfer",
                Amount = 200,
                EndBalance = 1000,
                DateOfTransaction = new DateTime(2016, 7, 2)
            };


             
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}