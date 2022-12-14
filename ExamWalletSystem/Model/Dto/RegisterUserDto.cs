using System;
using System.ComponentModel.DataAnnotations;

namespace ExamWalletSystem.Model.Dto
{
    public class RegisterUserDto
    {
        [MaxLength(30)]
        [Required]
        [DataType(DataType.Text, ErrorMessage = "Username is Required")]
        public string UserName { get; set; }
        [MaxLength(30)]
        [Required]
        [DataType(DataType.Password, ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}
