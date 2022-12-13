using AutoMapper;
using ExamWalletSystem.DBContext;
using ExamWalletSystem.Interface;
using ExamWalletSystem.Model;
using ExamWalletSystem.Model.Dto;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExamWalletSystem.Repository
{
    public class AccountRepository : IAccount
    {
        private readonly WalletSystemDBContext _context; 
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private User _user;
        private readonly string _pepper;
        private readonly int _iteration = 3;
        public AccountRepository(WalletSystemDBContext context, IConfiguration configuration, IMapper mapper)
        {
            this._context = context;
            this._configuration = configuration;
            this._mapper = mapper;
            _pepper = Environment.GetEnvironmentVariable("PasswordHashPepper");
        }

        public async Task<TokenResponseDto> Login(RegisterUserDto userDto)
        {
            bool isValidUser = false;
            bool isUserExist = false;
            bool isValidPassword = false;


            isUserExist = await CheckUserName(userDto.UserName);
            isValidPassword = await VerifyPassword(userDto);

            if (isValidPassword == true || isValidUser == true)
            {
                _user = await _context.tblUser.FirstOrDefaultAsync(u => u.UserName == userDto.UserName);

                var token = await GenerateToken();
                return new TokenResponseDto
                {
                    Token = token,
                    UserId = _user.UserName,
                    RefreshToken = CreateResfreshToken()
                };
            }

            return new TokenResponseDto
            {
                Token = null,
                UserId = null,
                RefreshToken = null
            };

        }
        public async Task<bool> Register(RegisterUserDto model)
        {
            string generateSalt = GenerateSalt();
            var HashedPassword = ComputeHash(model.Password, generateSalt, _pepper, _iteration);
            
            bool isUserExist = await CheckUserName(model.UserName);
            if (!isUserExist)
            {
                var user = _mapper.Map<User>(model);
                user.Password = HashedPassword;
                user.PasswordSalt = generateSalt;
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        } 
        private async Task<bool> CheckUserName(string userName) {
            var query = await _context.tblUser.FirstOrDefaultAsync(u => u.UserName == userName);
            if (query == null)
            {
                return false;
            }
            return  true;
        }
        private async Task<bool> VerifyPassword(RegisterUserDto userDto)
        { 
            var user = await _context.tblUser.FirstOrDefaultAsync(u => u.UserName == userDto.UserName);
            if (user == null)
            {
                throw new Exception("Username does not exist.");
            }

            var passwordHash = ComputeHash(userDto.Password, user.PasswordSalt, _pepper, _iteration);

            if (user.Password != passwordHash) { 
                throw new Exception("Username or password did not match.");
            }

            return true;
        } 
        private Task<string> GenerateToken()
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

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        } 
        private string CreateResfreshToken()
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
        private static string ComputeHash(string password, string salt, string pepper, int iteration)
        {
            if (iteration <= 0) return password;

            using var sha256 = SHA256.Create();
            var passwordSaltPepper = $"{password}{salt}{pepper}";
            var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
            var byteHash = sha256.ComputeHash(byteValue);
            var hash = Convert.ToBase64String(byteHash);
            return ComputeHash(hash, salt, pepper, iteration - 1);
        }
        private static string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var byteSalt = new byte[16];
            rng.GetBytes(byteSalt);
            var salt = Convert.ToBase64String(byteSalt);
            return salt;
        }
    }
}
