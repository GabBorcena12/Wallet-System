using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace ExamWalletSystem.Model
{
    public class Transaction
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string AccountNumberFrom { get; set; } 
        public string AccountNumberTo { get; set; }
        public string TransactionType { get; set; }
        public float Amount { get; set; }
        public float EndBalance { get; set; }
        public DateTime DateOfTransaction { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }

    }
}
