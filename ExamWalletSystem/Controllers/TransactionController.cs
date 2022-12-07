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
                return BadRequest($"Something went wrong with {nameof(GetUserTransaction)}. Error Message: {ex.Message}.");
            }
        }

        [HttpPatch] 
        [Route("api/[controller]/Deposit")]
        public async Task<IActionResult> Deposit(DepositDto model)
        {
            try
            {
                var result = await _reposiitory.Deposit(model);

                if (result == null)
                { 
                    return BadRequest();
                }
                 
                return Ok($"You have successfully deposited $ { model.Amount } to this Account Number:  { model.AccountNumberTo} .");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong with {nameof(Deposit)}. Error Message: {ex.Message}.");
            } 
        }

        [HttpPatch]
        [Route("api/[controller]/Withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawDto model)
        {
            try
            {
                var result = await _reposiitory.Withdraw(model);

                if (result == null)
                {
                    return BadRequest();
                }

                return Ok($"You have successfully Withdraw $ {model.Amount} using this Account Number:  {model.AccountNumberFrom} .");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong with {nameof(Withdraw)}. Error Message: {ex.Message}.");
            }
        }

        [HttpPatch]
        [Route("api/[controller]/FundTransfer")]
        public async Task<IActionResult> FundTransfer(TransactDto model)
        {
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var result = await _reposiitory.FundTransfer(model, userId);

                if (result == null)
                {
                    return BadRequest();
                } 

                if (model.AccountNumberFrom == model.AccountNumberTo)
                {
                    return BadRequest();
                }

                return Ok($"You have successfully transfer $ {model.Amount}. to { model.AccountNumberTo}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Something went wrong with {nameof(FundTransfer)}. Error Message: {ex.Message}.");
            }
        }
    }
}
