using System;
using System.ComponentModel.DataAnnotations;

namespace ExamWalletSystem.Model.Dto
{
    public class TransactDto
    {
        [Required] 
        public long AccountNumberFrom { get; set; }
        [Required] 
        public long AccountNumberTo { get; set; }

        [Required] 
        public float Amount { get; set; } 
    }
}
