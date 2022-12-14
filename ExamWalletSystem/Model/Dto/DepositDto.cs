using System.ComponentModel.DataAnnotations;

namespace ExamWalletSystem.Model.Dto
{
    public class DepositDto
    {
        [Required] 
        public long AccountNumberTo { get; set; } 
        [Required]
        public float Amount { get; set; }
    }
}
