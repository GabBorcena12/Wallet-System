using ExamWalletSystem.Model.Dto;
using System.Threading.Tasks;

namespace ExamWalletSystem.Interface
{
    public interface ITransaction
    {
        Task<TransactionDto> GetTransaction(string UserName);
        Task<AuthResponseDto> Deposit(TransactionDto transactionDto);
        Task<AuthResponseDto> Withdraw(TransactionDto transactionDto);
        Task<AuthResponseDto> FundTransfer(TransactionDto transactionDto);
    }
}
