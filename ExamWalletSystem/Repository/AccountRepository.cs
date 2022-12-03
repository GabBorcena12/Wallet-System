using ExamWalletSystem.DBContext;
using ExamWalletSystem.Interface;
using ExamWalletSystem.Model;
using ExamWalletSystem.Model.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExamWalletSystem.Repository
{
    public class AccountRepository : IAccount
    {
        private readonly WalletSystemDBContext _context; 
        private readonly IConfiguration _configuration;
        private User _user;
        public AccountRepository(WalletSystemDBContext context, IConfiguration configuration)
        {
            this._context = context;
            this._configuration = configuration; 
        }

        public async Task<bool> CheckUserName(UserDto userDto) {
            var query = await _context.tblUser.FindAsync(userDto.UserName);
            if (query == null)
            {
                return false;
            }
            return  true;
        }
        public async Task<bool> VerifyPassword(UserDto userDto)
        {
            var query = await _context.tblUser.FirstOrDefaultAsync(u => u.UserName == userDto.UserName && u.Password == userDto.Password);

            if (query == null)
            {
                return false;
            }
            return true;
        }
        public async Task<TokenResponseDto> Login(UserDto userDto)
        {
            bool isValidUser = false;
            bool isUserExist = false;
            bool isValidPassword = false;

            isUserExist = await CheckUserName(userDto);
            isValidPassword = await VerifyPassword(userDto);

            if (isValidPassword == true || isValidUser == true)
            {
                _user = await _context.tblUser.FindAsync(userDto.Id);
                var token = await GenerateToken();
                return new TokenResponseDto
                {
                    Token = token,
                    UserId = _user.UserName,
                    RefreshToken = await CreateResfreshToken()
                };
            }

            return null; 

        }

        public async Task<bool> Register(UserDto model)
        {
            try
            {
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();

                return true;
            }
            catch { 
                return false;
            }
        }

        public async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.UserName),
                new Claim("uid", _user.UserName),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /*public Task<AuthResponseDto> VerifyResfreshToken(AuthResponseDto authResponseDto)
        {
            throw new NotImplementedException();
        }*/

        public async Task<string> CreateResfreshToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);



            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.UserName),
                new Claim("uid", _user.UserName),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
