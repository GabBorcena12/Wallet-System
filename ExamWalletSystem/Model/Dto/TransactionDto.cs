using System;

namespace ExamWalletSystem.Model.Dto
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string AccountNumberFrom { get; set; }
        public string AccountNumberTo { get; set; }
        public string TransactionType { get; set; }
        public float Amount { get; set; }
        public float EndBalance { get; set; }
        public DateTime DateOfTransaction { get; set; }
    }
}
