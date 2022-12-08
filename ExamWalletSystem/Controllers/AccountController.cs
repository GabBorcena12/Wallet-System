﻿using ExamWalletSystem.Interface;
using ExamWalletSystem.Model;
using ExamWalletSystem.Model.Dto;
using ExamWalletSystem.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExamWalletSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccount _repos;

        public AccountController(IAccount repos)
        {
            this._repos = repos;
        }

        //api/account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (userDto.UserName == "" || userDto.Password == "")
            {
                return BadRequest();
            }
            var errors = await _repos.Register(userDto);

            if (!errors)
            {
                return BadRequest();
            }

            return Ok();

        }


        //api/account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> login([FromBody] RegisterUserDto loginDto)
        {
            if (loginDto.UserName == "" || loginDto.Password == "")
            {
                return BadRequest();
            }
            var authResponse = await _repos.Login(loginDto); 

            //return errors when trying to login the user
            if (authResponse == null)
            { 
                return Unauthorized();
            }

            return Ok(authResponse);
        }
    }
}
