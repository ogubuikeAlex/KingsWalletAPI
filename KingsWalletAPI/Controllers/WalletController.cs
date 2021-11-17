using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KingsWalletAPI.Data.Interfaces;
using KingsWalletAPI.Model.DataTransferObjects.WalletControllerDTO;
using KingsWalletAPI.Model.Entites;
using KingsWalletAPI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KingsWalletAPI.Controllers
{
    [Route("api/Wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }
        
        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer(TransferDTO model)
        {
            if (model is null)
                return BadRequest("TransferDTO sent from client is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity("The model for transfer is not valid");

            var result = await _walletService.Transfer(model);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost("FundAccount")]
        public async Task<IActionResult> FundAccount(FundAccountDTO model)
        {
            if (model is null)
                return BadRequest("TransferDTO sent from client is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity("The model for transfer is not valid");

            var result = await _walletService.FundAccount(model);

            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPost("PayBill")]
        public async Task<IActionResult> PayBill(PayBillDTO model)
        {
            if (model is null)
                return BadRequest("TransferDTO sent from client is null");

            if (!ModelState.IsValid)
                return UnprocessableEntity("The model for transfer is not valid");

            var result = await _walletService.PayBills(model);

            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpGet("{id}")]
        public IActionResult ViewTransactions(Guid id)
        {
           var result = _walletService.ViewTransactions(id);

            if (!result.Success)
                return Ok(result.Object as IEnumerable<Transaction>);

            return NotFound(result.Message);
        }

        [HttpGet]
        public IActionResult ViewAllTransactions()
        {
            var result = _walletService.ViewAllTransactions();

            return Ok(result);
        }
    }
}
