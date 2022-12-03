using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamWalletSystem.Model
{
    public class User
    {
        Random random = new Random();
        
        public User()
        {
            AccountNumber = random.Next(1,999999999);
            Balance = 0;
            RegisterDate = DateTime.Now;    

        }

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text, ErrorMessage = "Username is Required")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password, ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        public long AccountNumber { get; set; }
        public float Balance { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
