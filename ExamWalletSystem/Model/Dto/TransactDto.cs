using System;

namespace ExamWalletSystem.Model.Dto
{
    public class TransactDto
    { 
        public string AccountNumberFrom { get; set; }
        public string AccountNumberTo { get; set; } 
        public float Amount { get; set; } 
    }
}
