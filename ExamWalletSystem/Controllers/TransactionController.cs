using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using ExamWalletSystem.Model;
using ExamWalletSystem.Model.Dto;
using ExamWalletSystem.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Threading.Tasks;
using ExamWalletSystem.Interface;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExamWalletSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransaction _reposiitory;

        public TransactionController(ITransaction reposiitory)
        {
            this._reposiitory = reposiitory;
        }

        [HttpGet] 
        [Route("api/[controller]/GetUserTransaction")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TransactionDto>>> GetUserTransaction()
        { 
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized();
                }

                var result =  await _reposiitory.GetTransaction(userId);  
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
        }

        [HttpPatch] 
        [Route("api/[controller]/Deposit")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Deposit([FromBody] DepositDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized();
                }
                var result = await _reposiitory.Deposit(model, userId);

                if (result.Status != 200)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = result.Message });
                }

                return StatusCode(StatusCodes.Status200OK, new { message = $"You have successfully deposited $ {model.Amount} to this Account Number:  {model.AccountNumberTo} ." });
                 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            } 
        }

        [HttpPatch]
        [Route("api/[controller]/Withdraw")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawDto model)
        {
            try
            { 
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized();
                }

                var result = await _reposiitory.Withdraw(model, userId);

                if (result.Status != 200)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = result.Message });
                } 

                return StatusCode(StatusCodes.Status200OK, new { message = $"You have successfully Withdraw $ {model.Amount} using this Account Number:  {model.AccountNumberFrom} ." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
        }

        [HttpPatch]
        [Route("api/[controller]/FundTransfer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> FundTransfer([FromBody] TransactDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }  

                if (model.AccountNumberFrom == model.AccountNumberTo)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "Account Numbers must not be the same." });
                }

                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized();
                }

                var result = await _reposiitory.FundTransfer(model, userId);

                if (result.Status != 200)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = result.Message });
                }

                return StatusCode(StatusCodes.Status200OK, new { message = $"You have successfully transfer $ {model.Amount} to {model.AccountNumberTo}" }); 

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
        }
    }
}
