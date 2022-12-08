using System;
using System.ComponentModel.DataAnnotations;

namespace ExamWalletSystem.Model.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } 
        public string Password { get; set; } 
        public string AccountNumber { get; set; }
        public float Balance { get; set; }
        public DateTime RegisterDate { get; set; }
        public string PasswordSalt { get; set; }
    }
}
