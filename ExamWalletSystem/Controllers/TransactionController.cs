using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using ExamWalletSystem.Model;
using ExamWalletSystem.Model.Dto;
using ExamWalletSystem.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ExamWalletSystem.Controllers
{
    [Route("api/[controller]")]
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
        public ActionResult<IEnumerable<TransactionDto>> GetUserTransaction()
        { 
            try
            {
                var result =  _reposiitory.GetTransaction("GABB0812");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong with {nameof(GetUserTransaction)}. Error Message: {ex.Message}.", statusCode: 500);
            }
        }

        [HttpPatch] 
        [Route("api/[controller]/Deposit")]
        public IActionResult Deposit(TransactionDto model)
        {
            try
            {
                var result = _reposiitory.Deposit(model);

                if (result == null)
                { 
                    return BadRequest();
                }
                 
                return Ok($"You have successfully deposited $ { model.Amount } using this Account Number:  { model.AccountNumberFrom} .");
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong with {nameof(Deposit)}. Error Message: {ex.Message}.", statusCode: 500);
            } 
        }

        [HttpPatch]
        [Route("api/[controller]/Withdraw")]
        public IActionResult Withdraw(TransactionDto model)
        {
            try
            {
                var result = _reposiitory.Withdraw(model);

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
        public IActionResult FundTransfer(TransactionDto model)
        {
            try
            {
                var result = _reposiitory.FundTransfer(model);

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
