using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.RegularExpressions;

namespace ExamWalletSystem.Model
{
    public class User
    {  
        public User()
        {
            AccountNumber = GenerateRandomString(12);
            Balance = 0;
            RegisterDate = DateTime.Now;     
        }

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DataType(DataType.Text, ErrorMessage = "Username is Required")]
        public string UserName { get; set; }
        [Required] 
        [DataType(DataType.Password, ErrorMessage = "Password is Required")]
        public string Password { get; set; }
        [MaxLength(12)]
        public long AccountNumber { get; set; }
        public float Balance { get; set; }
        public DateTime RegisterDate { get; set; }
        public string PasswordSalt { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }

        private static long GenerateRandomString(int size)
        { 

            Random r = new Random();
            string dateYear = DateTime.Now.Year.ToString("yy");
            string dateMonth = DateTime.Now.Month.ToString();
            string dateDay = DateTime.Now.Day.ToString(); 
           
            string GetCurrentDate = dateYear + dateMonth + dateDay;
             
            var generateId = GetCurrentDate + Guid.NewGuid().ToString().Replace("-", string.Empty);
            var randomCode = Regex.Replace(generateId, "[a-zA-Z]", string.Empty).Substring(0, 12);
            long generatedNumber = (long)Convert.ToDouble(randomCode); 
            return generatedNumber;
        }

    }
}
