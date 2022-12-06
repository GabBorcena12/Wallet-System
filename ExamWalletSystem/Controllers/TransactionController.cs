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

namespace ExamWalletSystem.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionReposiitory _reposiitory;

        public TransactionController(TransactionReposiitory reposiitory)
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

                var result =  await _reposiitory.GetTransaction(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong with {nameof(GetUserTransaction)}. Error Message: {ex.Message}.", statusCode: 500);
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
                return Problem($"Something went wrong with {nameof(Deposit)}. Error Message: {ex.Message}.", statusCode: 500);
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
                return Problem($"Something went wrong with {nameof(Withdraw)}. Error Message: {ex.Message}.", statusCode: 500);
            }
        }

        [HttpPatch]
        [Route("api/[controller]/FundTransfer")]
        public async Task<IActionResult> FundTransfer(TransactDto model)
        {
            try
            {
                var result = await _reposiitory.FundTransfer(model);

                if (result == null)
                {
                    return BadRequest();
                }

                return Ok($"You have successfully transfer $ {model.Amount}. to { model.AccountNumberTo}");
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong with {nameof(FundTransfer)}. Error Message: {ex.Message}.", statusCode: 500);
            }
        }
    }
}
