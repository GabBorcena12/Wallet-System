using ExamWalletSystem.Interface;
using ExamWalletSystem.Model.Dto;
using System.Threading.Tasks;

namespace ExamWalletSystem.Repository
{
    public class TransactionReposiitory : ITransaction
    {
        public Task<AuthResponseDto> Deposit(TransactionDto transactionDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<AuthResponseDto> FundTransfer(TransactionDto transactionDto)
        {
            throw new System.NotImplementedException();
        }

        public Task<TransactionDto> GetTransaction(string UserName)
        {
            throw new System.NotImplementedException();
        }

        public Task<AuthResponseDto> Withdraw(TransactionDto transactionDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
