using ExamWalletSystem.Model.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExamWalletSystem.Interface
{
    public interface IAccount
    {
        Task<bool> Register(UserDto model); 
        Task<TokenResponseDto> Login(UserDto loginDto);
        public Task<string> CreateResfreshToken();
        //public Task<AuthResponseDto> VerifyResfreshToken(AuthResponseDto authResponseDto);
    }
}
