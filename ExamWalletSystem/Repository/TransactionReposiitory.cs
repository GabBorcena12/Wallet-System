using AutoMapper;
using ExamWalletSystem.DBContext;
using ExamWalletSystem.Interface;
using ExamWalletSystem.Model;
using ExamWalletSystem.Model.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Threading.Tasks; 

namespace ExamWalletSystem.Repository
{
    public class TransactionReposiitory : ITransaction
    {
        private readonly WalletSystemDBContext _db;
        private readonly IMapper mapper;

        public TransactionReposiitory(WalletSystemDBContext db, IMapper mapper)
        {
            this._db = db;
            this.mapper = mapper;
        }

        public async Task<List<TransactionDto>> GetTransaction(string userId)
        {
            var query = await GetUserAccountNumber(userId);
            var result = await _db.tblTransaction.Where(q => q.AccountNumberFrom == query.AccountNumber || q.AccountNumberTo == query.AccountNumber).ToListAsync();
            var mapResult = mapper.Map<List<TransactionDto>>(result);
            return mapResult;
        }
        public async Task<AuthResponseDto> FundTransfer(TransactDto transactionDto, string userId)
        {
            var mapData = mapper.Map<Transaction>(transactionDto); 
            //Verify Account Number
            bool checkAccFrom = await CheckAccountNumber(transactionDto.AccountNumberFrom);
            bool checkAccTo = await CheckAccountNumber(transactionDto.AccountNumberTo);
            //Get Account Number Details
            var getAccountNumberTo = await _db.tblUser.Where(q => q.AccountNumber == transactionDto.AccountNumberTo).FirstOrDefaultAsync();
            var getAccountNumberFrom = await _db.tblUser.Where(q => q.AccountNumber == transactionDto.AccountNumberFrom).FirstOrDefaultAsync();
             
            //Check Balance available
            float checkBalance = getAccountNumberFrom.Balance - transactionDto.Amount;

            if (checkAccFrom == true && checkAccTo == true && transactionDto.Amount > 0  
            && checkBalance >= 0  && getAccountNumberFrom.UserName == userId)  
            {
                try
                {  
                    float balance = getAccountNumberTo.Balance + transactionDto.Amount;
                    float deductBalance = getAccountNumberFrom.Balance - transactionDto.Amount;

                    getAccountNumberTo.Balance = balance;
                    getAccountNumberFrom.Balance = deductBalance > 0? deductBalance:0;

                    mapData.EndBalance = deductBalance;
                    mapData.DateOfTransaction = DateTime.Now;
                    mapData.TransactionType = "Fund Transfer";

                    _db.tblUser.Update(getAccountNumberTo);
                    _db.tblUser.Update(getAccountNumberFrom);
                    await _db.tblTransaction.AddAsync(mapData);
                    await _db.SaveChangesAsync();

                    return new AuthResponseDto
                    {
                        Status = 1,
                        Message = "Transaction Successful"
                    };
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }        
        public async Task<AuthResponseDto> Deposit(DepositDto transactionDto)
        {
            var mapData = mapper.Map<Transaction>(transactionDto);
            var getAccountNumberTo = await _db.tblUser.Where(q => q.AccountNumber == transactionDto.AccountNumberTo).FirstOrDefaultAsync();
             
            try
            {
                //Insert Dynamic Data for Logging 
                float addBalance = getAccountNumberTo.Balance + transactionDto.Amount;
                getAccountNumberTo.Balance = addBalance > 0 ? addBalance : 0;

                mapData.EndBalance = addBalance;
                mapData.DateOfTransaction = DateTime.Now;
                mapData.TransactionType = "Deposit";

                _db.tblUser.Update(getAccountNumberTo);
                await _db.tblTransaction.AddAsync(mapData);
                await _db.SaveChangesAsync();

                return new AuthResponseDto
                {
                    Status = 1,
                    Message = "Transaction Successful"
                };
            }
            catch
            {
                return null;
            } 
        }
        public async Task<AuthResponseDto> Withdraw(WithdrawDto transactionDto)
        {
            var mapData = mapper.Map<Transaction>(transactionDto);
            var getAccountNumberFrom = await _db.tblUser.Where(q => q.AccountNumber == transactionDto.AccountNumberFrom).FirstOrDefaultAsync();

            //Check Balance available
            float checkBalance = getAccountNumberFrom.Balance - transactionDto.Amount;
            if (checkBalance >= 0)
            {
                try
                {
                    //Insert Dynamic Data for Logging 
                    float deductBalance = getAccountNumberFrom.Balance - transactionDto.Amount;
                    getAccountNumberFrom.Balance = deductBalance > 0 ? deductBalance : 0;

                    mapData.EndBalance = deductBalance;
                    mapData.DateOfTransaction = DateTime.Now;
                    mapData.TransactionType = "Withdraw";

                    _db.tblUser.Update(getAccountNumberFrom);
                    await _db.tblTransaction.AddAsync(mapData);
                    await _db.SaveChangesAsync();

                    return new AuthResponseDto
                    {
                        Status = 1,
                        Message = "Transaction Successful"
                    };
                }
                catch
                {
                    return null;
                }
            }
            else { 
                return null;
            }
        }
        private async Task<User> GetUserAccountNumber(string userId)
        {
            var isExists = await _db.tblUser.Where(q => q.UserName == userId).FirstOrDefaultAsync();
            if (isExists == null)
            {
                return null;
            }
            return isExists;
        }
        private async Task<bool> CheckAccountNumber(string accountNumber)
        {
            var isExists = await _db.tblUser.Where(q => q.AccountNumber == accountNumber).FirstOrDefaultAsync();
            if (isExists == null)
            {
                return false;
            }
            return true;
        }

    }
}
