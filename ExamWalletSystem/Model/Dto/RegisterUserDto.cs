using System;
using System.ComponentModel.DataAnnotations;

namespace ExamWalletSystem.Model.Dto
{
    public class RegisterUserDto
    {
        [MaxLength(30)]
        [Required]
        public string UserName { get; set; }
        [MaxLength(30)]
        [Required]
        public string Password { get; set; }
    }
}
