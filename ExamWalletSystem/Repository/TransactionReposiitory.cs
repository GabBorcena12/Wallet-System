using AutoMapper;
using ExamWalletSystem.DBContext;
using ExamWalletSystem.Interface;
using ExamWalletSystem.Model;
using ExamWalletSystem.Model.Dto;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

                    var getVersionFrom = await _db.tblUser.Where(q => q.UserName == getAccountNumberFrom.UserName).FirstOrDefaultAsync();
                    var getVersionTo = await _db.tblUser.Where(q => q.UserName == getAccountNumberTo.UserName).FirstOrDefaultAsync();

                    if (getVersionFrom.Version == getAccountNumberFrom.Version 
                        && getAccountNumberTo.Version == getAccountNumberTo.Version)
                    {
                        await _db.SaveChangesAsync();

                        return new AuthResponseDto
                        {
                            Status = 200,
                            Message = "Transaction Successful"
                        };
                    }
                    else
                    {
                        return new AuthResponseDto
                        {
                            Status = 409,
                            Message = "A conflict has occur."
                        };
                    }
                }
                catch(Exception ex)
                {
                    return new AuthResponseDto
                    {
                        Status = 400,
                        Message = ex.Message
                    };
                }
            }
            else
            {
                if (checkBalance <= 0)
                {
                    return new AuthResponseDto
                    {
                        Status = 400,
                        Message = "Balance not sufficient."
                    }; 
                } 

                if (getAccountNumberFrom.UserName != userId)
                {
                    return new AuthResponseDto
                    {
                        Status = 400,
                        Message = "Please input your correct Account Number."
                    };
                }

                return new AuthResponseDto
                {
                    Status = 400,
                    Message = "Please check your input data."
                };
            }
        }
        public async Task<AuthResponseDto> Deposit(DepositDto transactionDto, string userId)
        {
            var mapData = mapper.Map<Transaction>(transactionDto);
            var getAccountNumberTo = await _db.tblUser.Where(q => q.AccountNumber == transactionDto.AccountNumberTo).FirstOrDefaultAsync();

            if (transactionDto.AccountNumberTo <= 0 || transactionDto.Amount <= 0)
            {
                return new AuthResponseDto
                {
                    Status = 400,
                    Message = "Please check your data input before proceeding."
                };
            }

            if (getAccountNumberTo.UserName != userId) {
                return new AuthResponseDto
                {
                    Status = 400,
                    Message = "You can only deposit to your account."
                };
            }

            try
            { 
                float addBalance = getAccountNumberTo.Balance + transactionDto.Amount;
                getAccountNumberTo.Balance = addBalance > 0 ? addBalance : 0;

                mapData.EndBalance = addBalance;
                mapData.DateOfTransaction = DateTime.Now;
                mapData.TransactionType = "Deposit";

                 


                _db.tblUser.Update(getAccountNumberTo);
                await _db.tblTransaction.AddAsync(mapData);

                var getVersion = await _db.tblUser.Where(q => q.UserName == getAccountNumberTo.UserName).FirstOrDefaultAsync();
                
                if (getVersion.Version == getAccountNumberTo.Version)
                {
                    await _db.SaveChangesAsync();

                    return new AuthResponseDto
                    {
                        Status = 200,
                        Message = "Transaction Successful"
                    };
                }
                else {
                    return new AuthResponseDto
                    {
                        Status = 409,
                        Message = "A conflict has occur."
                    };
                }


                
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Status = 400,
                    Message = ex.Message
                };
            }
        }
        
        public async Task<AuthResponseDto> Withdraw(WithdrawDto transactionDto,string userId)
        {
            var mapData = mapper.Map<Transaction>(transactionDto);

            var getAccountNumberFrom = await _db.tblUser.Where(q => q.AccountNumber == transactionDto.AccountNumberFrom).FirstOrDefaultAsync();

            //Check Balance available
            float checkBalance = getAccountNumberFrom.Balance - transactionDto.Amount;

            if (getAccountNumberFrom.UserName != userId)
            {
                return new AuthResponseDto
                {
                    Status = 400,
                    Message = "You can only deposit to your account."
                };
            }

            if (transactionDto.AccountNumberFrom <= 0 || transactionDto.Amount <= 0) {
                return new AuthResponseDto
                {
                    Status = 400,
                    Message = "Please check your data input before proceeding."
                };
            }
            if (checkBalance >= 0)
            {
                try
                { 
                    float deductBalance = getAccountNumberFrom.Balance - transactionDto.Amount;
                    getAccountNumberFrom.Balance = deductBalance > 0 ? deductBalance : 0;

                    mapData.EndBalance = deductBalance;
                    mapData.DateOfTransaction = DateTime.Now;
                    mapData.TransactionType = "Withdraw";

                    _db.tblUser.Update(getAccountNumberFrom);
                    await _db.tblTransaction.AddAsync(mapData);

                    var getVersion = await _db.tblUser.Where(q => q.UserName == getAccountNumberFrom.UserName).FirstOrDefaultAsync();

                    if (getVersion.Version == getAccountNumberFrom.Version)
                    {
                        await _db.SaveChangesAsync();

                        return new AuthResponseDto
                        {
                            Status = 200,
                            Message = "Transaction Successful"
                        };
                    }
                    else
                    {
                        return new AuthResponseDto
                        {
                            Status = 409,
                            Message = "A conflict has occur."
                        };
                    }
                }
                catch(Exception ex)
                {
                    return new AuthResponseDto
                    {
                        Status = 400,
                        Message = ex.Message
                    };
                }
            }
            else {
                return new AuthResponseDto
                {
                    Status = 400,
                    Message = "Balance not sufficient."
                };
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
        private async Task<bool> CheckAccountNumber(long accountNumber)
        {
            var isExists = await _db.tblUser.Where(q => q.AccountNumber == accountNumber).FirstOrDefaultAsync();
            if (isExists == null)
            {
                throw new Exception($"Account Number doesn't exist.");
            }
            return true;
        } 

    }
}
