using ExamWalletSystem.Model.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExamWalletSystem.Interface
{
    public interface IAccount
    {
        Task<bool> Register(RegisterUserDto model); 
        Task<TokenResponseDto> Login(RegisterUserDto loginDto);
        //public string CreateResfreshToken();
    }
}
