using ExamWalletSystem.Model.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExamWalletSystem.Interface
{
    public interface ITransaction
    {
        Task<List<TransactionDto>> GetTransaction(string userId);
        Task<AuthResponseDto> Deposit(DepositDto transactionDto);
        Task<AuthResponseDto> Withdraw(WithdrawDto transactionDto);
        Task<AuthResponseDto> FundTransfer(TransactDto transactionDto, string UserId);
    }
}
