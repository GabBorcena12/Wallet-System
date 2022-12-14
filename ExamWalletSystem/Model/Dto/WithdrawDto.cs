using System.ComponentModel.DataAnnotations;

namespace ExamWalletSystem.Model.Dto
{
    public class WithdrawDto
    { 
        [Required]
        public long AccountNumberFrom { get; set; }
        [Required]
        public float Amount { get; set; }
    }
}
