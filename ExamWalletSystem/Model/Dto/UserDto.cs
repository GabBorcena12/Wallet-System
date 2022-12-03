using System;

namespace ExamWalletSystem.Model.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long AccountNumber { get; set; }
        public float Balance { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
